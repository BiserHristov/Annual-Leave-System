namespace AnnualLeaveSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services.Emails;
    using AnnualLeaveSystem.Services.EmployeeLeaveTypes;
    using AnnualLeaveSystem.Services.Employees;
    using AnnualLeaveSystem.Services.Holidays;
    using AnnualLeaveSystem.Services.Leaves;
    using AnnualLeaveSystem.Services.LeaveTypes;
    using AnnualLeaveSystem.Services.Teams;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using static WebConstants;

    //[Authorize]
    public class LeavesController : Controller
    {
        private readonly IEmployeeLeaveTypesService employeeLeaveTypesService;
        private readonly IEmailSenderService emailSenderService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IEmployeeService employeeService;
        private readonly ILeaveService leaveService;
        private readonly ITeamService teamService;
        private readonly IHolidayService holidayService;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        private readonly UserManager<Employee> userManager;

        public LeavesController(
            ILeaveService leaveService,
            ITeamService teamService,
            ILeaveTypeService leaveTypeService,
            UserManager<Employee> userManager,
            IEmployeeLeaveTypesService employeeLeaveTypesService,
            IEmployeeService employeeService,
            IMapper mapper,
            IEmailSenderService emailSenderService,
            IHolidayService holidayService, 
            IMemoryCache cache)
        {
            this.leaveService = leaveService;
            this.teamService = teamService;
            this.leaveTypeService = leaveTypeService;
            this.userManager = userManager;
            this.employeeLeaveTypesService = employeeLeaveTypesService;
            this.employeeService = employeeService;
            this.mapper = mapper;
            this.emailSenderService = emailSenderService;
            this.holidayService = holidayService;
            this.cache = cache;
        }

        public IActionResult Add()
        {
            var model = new LeaveFormModel
            {
                LeaveTypes = this.leaveService.GetLeaveTypes(),
                EmployeesInTeam = this.leaveService.GetEmployeesInTeam(this.User.GetId()),
                ОfficialHolidays = this.leaveService.GetHolidays(),
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(LeaveFormModel leaveModel)
        {
            if (leaveModel.StartDate > leaveModel.EndDate)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), "Start date should be before end date.");
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), "End date should be after start date.");
            }

            if (leaveModel.StartDate < DateTime.UtcNow.Date)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), "Start date should be after or equal to todays' date.");
            }

            if (leaveModel.EndDate < DateTime.UtcNow.Date)
            {
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), "End date should be after or equal to todays' date.");
            }

            var (isHoliday, name) = this.holidayService.IsHoliday(leaveModel.StartDate);

            if (isHoliday)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), $"This date is official holiday ({name})");
            }

            (isHoliday, name) = this.holidayService.IsHoliday(leaveModel.EndDate);

            if (isHoliday)
            {
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), $"This date is official holiday ({name})");
            }

            var allHolidays = this.holidayService.AllDates();
            
            var businessDaysCount = GetBusinessDays(leaveModel.StartDate, leaveModel.EndDate, allHolidays);

            if (leaveModel.TotalDays != businessDaysCount || leaveModel.TotalDays == 0)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "Count of days is not correct or it is equal to zero.");
            }

            var leaveTypeExist = this.leaveTypeService.TypeExist(leaveModel.LeaveTypeId);

            if (!leaveTypeExist)
            {
                this.ModelState.AddModelError(nameof(leaveModel.LeaveTypeId), "Leave type does not exist.");
            }

            var employee = this.userManager.FindByIdAsync(this.User.GetId()).GetAwaiter().GetResult();

            var employeeExist = this.teamService.EmployeeExistInTeam(employee.TeamId, this.User.GetId());

            if (!employeeExist)
            {
                this.ModelState.AddModelError(nameof(leaveModel.SubstituteEmployeeId), "There is no such employee in your team.");
            }

            var employeeLeave = employeeLeaveTypesService.GetLeaveType(this.User.GetId(), leaveModel.LeaveTypeId);

            if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays - employeeLeave.PendingApprovalDays < leaveModel.TotalDays)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "You do no have enough days left from the selected leave type option.");
            }

            var leaves = this.leaveService.GetNotFinishedLeaves(this.User.GetId());

            foreach (var currentLeave in leaves)
            {
                var isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate);
                var isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate);

                if (isStartDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.StartDate), "You already have Leave Request for this date.");
                }

                if (isEndDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.EndDate), "You already have Leave Request for this date.");
                }

                if (isStartDateTaken || isEndDateTaken)
                {
                    break;
                }
            }

            var substituteLeaves = this.leaveService.GetSubstituteApprovedLeaves(this.User.GetId());

            foreach (var currentLeave in substituteLeaves)
            {
                var isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate);
                var isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate);

                if (isStartDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.StartDate), "You are substitute for this date.");
                }

                if (isEndDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.EndDate), "You are substitute for this date.");
                }

                if (isStartDateTaken || isEndDateTaken)
                {
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                leaveModel.LeaveTypes = this.leaveService.GetLeaveTypes();
                leaveModel.EmployeesInTeam = this.leaveService.GetEmployeesInTeam(this.User.GetId());
                return View(leaveModel);
            }

            var approveEmployeeId = this.employeeService.TeamLeadId(this.User.GetId());

            this.leaveService.Create(
                leaveModel.StartDate.Date,
                leaveModel.EndDate.Date,
                leaveModel.TotalDays,
                leaveModel.LeaveTypeId,
                this.User.GetId(),
                leaveModel.SubstituteEmployeeId,
                approveEmployeeId,
                leaveModel.Comments,
                leaveModel.RequestDate);

            var model = this.mapper.Map<EmailServiceModel>(leaveModel);

            model.RequestEmployeeName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;

            string message = $"A leave request is waiting for your approval: {model}";

            this.emailSenderService.SendEmail(EmailRequestSubject, message);

            if (this.User.IsUser())
            {
                return RedirectToAction("History", "Statistics");
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] AllLeavesQueryModel query)
        {
            var queryResult = this.leaveService.All(
                query.Status,
                query.FirstName,
                query.LastName,
                query.Sorting,
                query.CurrentPage,
                AllLeavesQueryModel.LeavesPerPage,
                this.User.IsTeamLead(),
                this.User.GetId());

            var statuses = Enum.GetValues(typeof(Status))
                               .Cast<Status>()
                               .ToList();

            var totalLeaves = queryResult.TotalLeaves;

            query.Statuses = statuses;
            query.TotalLeaves = totalLeaves;
            query.Leaves = queryResult.Leaves;

            return View(query);
        }

        public IActionResult Details(int leaveId)
        {
            if (leaveId == 0)
            {
                return this.NotFound();
            }

            var leave = this.leaveService.GetLeaveById(leaveId);

            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        public IActionResult Edit(int leaveId)
        {
            if (!this.leaveService.Exist(leaveId))
            {
                return BadRequest();
            }

            var leave = this.leaveService.GetLeave(leaveId);

            if (!this.User.IsAdmin() && leave.RequestEmployeeId != this.User.GetId())
            {
                return Unauthorized();
            }

            leave.LeaveTypes = this.leaveService.GetLeaveTypes();
            leave.EmployeesInTeam = this.leaveService.GetEmployeesInTeam(leave.RequestEmployeeId);

            var leaveForm = this.mapper.Map<LeaveFormModel>(leave);
            leaveForm.ApproveEmployeeId = leave.ApproveEmployeeId ?? leave.RequestEmployeeId;

            return View(leaveForm);
        }

        [HttpPost]
        public IActionResult Edit(int leaveId, LeaveFormModel leaveModel)
        {
            if (!this.User.IsAdmin() && !this.leaveService.IsOwn(leaveId, this.User.GetId()))
            {
                return BadRequest();
            }

            if (leaveModel.StartDate > leaveModel.EndDate)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), "Start date should be before end date.");
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), "End date should be after start date.");
            }

            if (leaveModel.StartDate < DateTime.UtcNow.Date)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), "Start date should be after or equal to todays' date.");
            }

            if (leaveModel.EndDate < DateTime.UtcNow.Date)
            {
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), "End date should be after or equal to todays' date.");
            }

            var (isHoliday, name) = this.holidayService.IsHoliday(leaveModel.StartDate);

            if (isHoliday)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), $"This date is official holiday ({name})");
            }

            (isHoliday, name) = this.holidayService.IsHoliday(leaveModel.EndDate);

            if (isHoliday)
            {
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), $"This date is official holiday ({name})");
            }


            var allHolidays = this.holidayService.AllDates();

            var businessDaysCount = GetBusinessDays(leaveModel.StartDate, leaveModel.EndDate, allHolidays);

            if (leaveModel.TotalDays != businessDaysCount || leaveModel.TotalDays == 0)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "Count of days is not correct or it is equal to zero.");
            }

            var leaveTypeExist = this.leaveTypeService.TypeExist(leaveModel.LeaveTypeId);

            if (!leaveTypeExist)
            {
                this.ModelState.AddModelError(nameof(leaveModel.LeaveTypeId), "Leave type does not exist.");
            }

            var teamId = this.employeeService.TeamId(leaveModel.RequestEmployeeId);

            var employeeExist = this.teamService.EmployeeExistInTeam(teamId, this.User.GetId());

            if (!this.User.IsAdmin() && !employeeExist)
            {
                this.ModelState.AddModelError(nameof(leaveModel.SubstituteEmployeeId), "There is no such employee in your team.");
            }

            var employeeLeave = this.employeeLeaveTypesService.GetLeaveType(leaveModel.RequestEmployeeId, leaveModel.LeaveTypeId);
            var previousLeaveTypeId = this.leaveService.GetLeaveTypeId(leaveId);
            var previousLeaveTotalDays = this.leaveService.GetLeaveTotalDays(leaveId);

            if (leaveModel.LeaveTypeId == previousLeaveTypeId)
            {
                if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays - (employeeLeave.PendingApprovalDays - previousLeaveTotalDays) < leaveModel.TotalDays)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "You do no have enough days left from the selected leave type option.");
                }
            }
            else
            {
                if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays - employeeLeave.PendingApprovalDays < leaveModel.TotalDays)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "You do no have enough days left from the selected leave type option.");
                }
            }

            var leaves = this.leaveService.GetNotFinishedLeaves(leaveModel.RequestEmployeeId);

            foreach (var currentLeave in leaves)
            {
                var isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate) && currentLeave.Id != leaveId;
                var isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate) && currentLeave.Id != leaveId;

                if (isStartDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.StartDate), "You already have Leave Request for this date.");
                }

                if (isEndDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.EndDate), "You already have Leave Request for this date.");
                }

                if (isStartDateTaken || isEndDateTaken)
                {
                    break;
                }
            }

            var substituteLeaves = this.leaveService.GetSubstituteApprovedLeaves(leaveModel.RequestEmployeeId);

            foreach (var currentLeave in substituteLeaves)
            {
                var isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate);
                var isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate);

                if (isStartDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.StartDate), "You are substitute for this date.");
                }

                if (isEndDateTaken)
                {
                    this.ModelState.AddModelError(nameof(leaveModel.EndDate), "You are substitute for this date.");
                }

                if (isStartDateTaken || isEndDateTaken)
                {
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                leaveModel.LeaveTypes = this.leaveService.GetLeaveTypes();
                leaveModel.EmployeesInTeam = this.leaveService.GetEmployeesInTeam(leaveModel.RequestEmployeeId);
                return View(leaveModel);
            }

            var leaveIsEdited = this.leaveService.Edit(
               leaveId,
               leaveModel.StartDate.Date,
               leaveModel.EndDate.Date,
               leaveModel.TotalDays,
               leaveModel.LeaveTypeId,
               leaveModel.RequestEmployeeId,
               leaveModel.SubstituteEmployeeId,
               leaveModel.ApproveEmployeeId,
               leaveModel.Comments);

            if (this.User.IsInRole(UserRoleName))
            {
                return RedirectToAction("History", "Statistics");
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public IActionResult Cancel(int leaveId)
        {
            var leaveExist = leaveService.Exist(leaveId);

            if (!leaveExist)
            {
                return BadRequest();
            }

            leaveService.Cancel(leaveId);

            SendMailStatusChange(Status.Canceled.ToString(), leaveId);

            if (this.User.IsInRole(UserRoleName))
            {
                return RedirectToAction("History", "Statistics");
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public IActionResult Reject(int leaveId)
        {
            var leaveExist = leaveService.Exist(leaveId);

            if (!leaveExist)
            {
                return BadRequest();
            }

            leaveService.Reject(leaveId);

            SendMailStatusChange(Status.Rejected.ToString(), leaveId);

            if (this.User.IsInRole(UserRoleName))
            {
                return RedirectToAction("History", "Statistics");
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForApproval()
        {
            var leaves = leaveService.LeavesForApproval(this.User.GetId(), this.User.IsTeamLead());

            return View(leaves);
        }

        [HttpPost]
        public IActionResult ForApproval(int leaveId)
        {
            if (!this.leaveService.Exist(leaveId))
            {
                return BadRequest();
            }

            leaveService.Approve(leaveId, this.User.IsUser());

            SendMailStatusChange(Status.Approved.ToString(), leaveId);

            return RedirectToAction(nameof(ForApproval));
        }

        private static double GetBusinessDays(DateTime startDate, DateTime endDate, IEnumerable<string> holidays)
        {
            double calcBusinessDays = 1 + ((endDate - startDate).TotalDays * 5 - (startDate.DayOfWeek - endDate.DayOfWeek) * 2) / 7;

            if (endDate.DayOfWeek == DayOfWeek.Saturday)
            {
                calcBusinessDays--;
            }

            if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                calcBusinessDays--;
            }

            while (startDate <= endDate)
            {
                if (holidays.Contains(startDate.ToLocalTime().Date.ToString("dd.MM.yyy")) &&
                    startDate.DayOfWeek != DayOfWeek.Saturday &&
                    startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    calcBusinessDays--;
                }

                startDate=startDate.AddDays(1);
            }
            return calcBusinessDays;
        }

        private static bool IsInRange(DateTime currentDate, DateTime startDate, DateTime endDate)
        {
            return currentDate >= startDate && currentDate <= endDate;
        }

        private void SendMailStatusChange(string status, int leaveId)
        {
            string message = GetMessage(status, leaveId);

            this.emailSenderService.SendEmail(EmailStatusChanged, message);
        }

        private string GetMessage(string status, int leaveId)
        {
            var leaveModel = leaveService.GetLeaveById(leaveId);

            var model = new EmailServiceModel
            {
                StartDate = leaveModel.StartDate.Date.ToString("dd.MM.yyyy"),
                EndDate = leaveModel.EndDate.Date.ToString("dd.MM.yyyy"),
                RequestEmployeeName = leaveModel.RequestEmployeeName,
                TotalDays = leaveModel.TotalDays,
            };

            string message = $"Your request is {status}. {model}";
            return message;
        }
    }
}
