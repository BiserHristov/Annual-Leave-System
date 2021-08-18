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
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Caching.Memory;
    using static WebConstants;
    using static WebConstants.Email;
    using static WebConstants.Leaves;

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

        [Authorize]
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
        [Authorize]
        public IActionResult Add(LeaveFormModel leaveModel)
        {
            ValidateLeave(
                leaveModel,
                this.ModelState,
                holidayService,
                leaveTypeService,
                employeeService,
                employeeLeaveTypesService,
                leaveService,
                teamService,
                this.User.GetId(),
                0,
                false,
                this.User.IsAdmin());

            if (!this.ModelState.IsValid)
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

            model.RequestEmployeeName = employeeService.FullName(this.User.GetId());

            string message = ContentText + model;

            this.emailSenderService.SendEmail(RequestSubject, message);

            if (this.User.IsUser())
            {
                return RedirectToAction("History", "Statistics");
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
        [Authorize]
        public IActionResult Edit(int leaveId, LeaveFormModel leaveModel)
        {
            if (!this.User.IsAdmin() && !this.leaveService.IsOwn(leaveId, this.User.GetId()))
            {
                return BadRequest();
            }

            ValidateLeave(
             leaveModel,
             this.ModelState,
             holidayService,
             leaveTypeService,
             employeeService,
             employeeLeaveTypesService,
             leaveService,
             teamService,
             leaveModel.RequestEmployeeId,
             leaveId,
             true,
             this.User.IsAdmin());

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
        [Authorize]
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
        [Authorize]
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

        [Authorize]
        public IActionResult ForApproval()
        {
            var leaves = leaveService.LeavesForApproval(this.User.GetId(), this.User.IsTeamLead());

            return View(leaves);
        }

        [HttpPost]
        [Authorize]
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

        private static void ValidateLeave(
            LeaveFormModel leaveModel,
            ModelStateDictionary modelState,
            IHolidayService holidayService,
            ILeaveTypeService leaveTypeService,
            IEmployeeService employeeService,
            IEmployeeLeaveTypesService employeeLeaveTypesService,
            ILeaveService leaveService,
            ITeamService teamService,
            string employeeId,
            int leaveId,
            bool isInEdit,
            bool isAdmin)
        {
            if (leaveModel.StartDate > leaveModel.EndDate)
            {
                modelState.AddModelError(nameof(leaveModel.StartDate), StartBeforeEndDateMessage);
                modelState.AddModelError(nameof(leaveModel.EndDate), EndAfterStartDateMessage);
            }

            if (IsBeforeToday(leaveModel.StartDate))
            {
                modelState.AddModelError(nameof(leaveModel.StartDate), "Start date" + AfterOrEqualTodayMessage);
            }

            if (IsBeforeToday(leaveModel.EndDate))
            {
                modelState.AddModelError(nameof(leaveModel.EndDate), "End date" + AfterOrEqualTodayMessage);
            }

            var (isHoliday, name) = holidayService.IsHoliday(leaveModel.StartDate);

            if (isHoliday)
            {
                modelState.AddModelError(nameof(leaveModel.StartDate), string.Format(OfficialHolidayMessage, name));
            }

            (isHoliday, name) = holidayService.IsHoliday(leaveModel.EndDate);

            if (isHoliday)
            {
                modelState.AddModelError(nameof(leaveModel.EndDate), string.Format(OfficialHolidayMessage, name));
            }

            var allHolidays = holidayService.AllDates();

            var businessDaysCount = GetBusinessDays(leaveModel.StartDate, leaveModel.EndDate, allHolidays);

            if (leaveModel.TotalDays != businessDaysCount || leaveModel.TotalDays == 0)
            {
                modelState.AddModelError(nameof(leaveModel.TotalDays), IncorrectTotalDaysMessage);
            }

            var leaveTypeExist = leaveTypeService.TypeExist(leaveModel.LeaveTypeId);

            if (!leaveTypeExist)
            {
                modelState.AddModelError(nameof(leaveModel.LeaveTypeId), IncorrectLeaveTypeMessage);
            }

            var teamId = employeeService.TeamId(employeeId);

            var employeeExist = teamService.EmployeeExistInTeam(teamId, employeeId);

            var employeeLeave = employeeLeaveTypesService.GetLeaveType(employeeId, leaveModel.LeaveTypeId);

            if (isInEdit)
            {
                if (!isAdmin && !employeeExist)
                {
                    modelState.AddModelError(nameof(leaveModel.SubstituteEmployeeId), NotExistingEmployeeInTeamMessage);
                }

                var previousLeaveTypeId = leaveService.GetLeaveTypeId(leaveId);
                var previousLeaveTotalDays = leaveService.GetLeaveTotalDays(leaveId);

                if (leaveModel.LeaveTypeId == previousLeaveTypeId)
                {
                    if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays - (employeeLeave.PendingApprovalDays - previousLeaveTotalDays) < leaveModel.TotalDays)
                    {
                        AddTotalDaysModelError(modelState, leaveModel);
                    }
                }
                else
                {
                    if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays - employeeLeave.PendingApprovalDays < leaveModel.TotalDays)
                    {
                        AddTotalDaysModelError(modelState, leaveModel);
                    }
                }
            }
            else
            {
                if (!employeeExist)
                {
                    modelState.AddModelError(nameof(leaveModel.SubstituteEmployeeId), NotExistingEmployeeInTeamMessage);
                }

                if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays - employeeLeave.PendingApprovalDays < leaveModel.TotalDays)
                {
                    AddTotalDaysModelError(modelState, leaveModel);
                }
            }

            var leaves = leaveService.GetNotFinishedLeaves(employeeId);

            foreach (var currentLeave in leaves)
            {
                bool isStartDateTaken;
                bool isEndDateTaken;

                if (isInEdit)
                {
                    isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate) && currentLeave.Id != leaveId;
                    isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate) && currentLeave.Id != leaveId;
                }
                else
                {
                    isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate);
                    isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate);
                }

                var existingDateInPeriod = IsInRange(currentLeave.StartDate, leaveModel.StartDate, leaveModel.EndDate) ||
                                       IsInRange(currentLeave.EndDate, leaveModel.StartDate, leaveModel.EndDate);

                if (existingDateInPeriod)
                {
                    modelState.AddModelError(nameof(leaveModel.StartDate), ExistingRequestInsidePeriodMessage);
                    modelState.AddModelError(nameof(leaveModel.EndDate), ExistingRequestInsidePeriodMessage);
                }

                if (isStartDateTaken)
                {
                    modelState.AddModelError(nameof(leaveModel.StartDate), ExistingRequestForDateMessage);
                }

                if (isEndDateTaken)
                {
                    modelState.AddModelError(nameof(leaveModel.EndDate), ExistingRequestForDateMessage);
                }

                if (isStartDateTaken || isEndDateTaken || existingDateInPeriod)
                {
                    break;
                }
            }

            var substituteLeaves = leaveService.GetSubstituteApprovedLeaves(employeeId);

            foreach (var currentLeave in substituteLeaves)
            {
                var isStartDateTaken = IsInRange(leaveModel.StartDate, currentLeave.StartDate, currentLeave.EndDate);
                var isEndDateTaken = IsInRange(leaveModel.EndDate, currentLeave.StartDate, currentLeave.EndDate);

                if (isStartDateTaken)
                {
                    modelState.AddModelError(nameof(leaveModel.StartDate), AlreadySubstituteForDateMessage);
                }

                if (isEndDateTaken)
                {
                    modelState.AddModelError(nameof(leaveModel.EndDate), AlreadySubstituteForDateMessage);
                }

                if (isStartDateTaken || isEndDateTaken)
                {
                    break;
                }
            }
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
                if (holidays.Contains(startDate.ToLocalTime().Date.ToString(DateFormat)) &&
                    startDate.DayOfWeek != DayOfWeek.Saturday &&
                    startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    calcBusinessDays--;
                }

                startDate = startDate.AddDays(1);

            }

            return calcBusinessDays;
        }

        private static bool IsInRange(DateTime currentDate, DateTime startDate, DateTime endDate)
            => currentDate >= startDate && currentDate <= endDate;

        private static bool IsBeforeToday(DateTime date)
            => date < DateTime.UtcNow.Date;

        private static void AddTotalDaysModelError(ModelStateDictionary modelState, LeaveFormModel leaveModel)
        {
            modelState.AddModelError(nameof(leaveModel.TotalDays), InsufficientLeaveDaysLeftMessage);
        }

        private void SendMailStatusChange(string status, int leaveId)
        {
            string message = GetMessage(status, leaveId);

            this.emailSenderService.SendEmail(StatusChanged, message);
        }

        private string GetMessage(string status, int leaveId)
        {
            var leaveModel = leaveService.GetLeaveById(leaveId);

            var model = new EmailServiceModel
            {
                StartDate = leaveModel.StartDate.Date.ToString(DateFormat),
                EndDate = leaveModel.EndDate.Date.ToString(DateFormat),
                RequestEmployeeName = leaveModel.RequestEmployeeName,
                TotalDays = leaveModel.TotalDays,
            };

            string message = $"Your request is {status}. {model}";
            return message;
        }
    }
}
