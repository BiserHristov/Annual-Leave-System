namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services.EmployeeLeaveTypes;
    using AnnualLeaveSystem.Services.Employees;
    using AnnualLeaveSystem.Services.Leaves;
    using AnnualLeaveSystem.Services.LeaveTypes;
    using AnnualLeaveSystem.Services.Teams;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using static AnnualLeaveSystem.Data.DataConstants.User;
    using static WebConstants;

    [Authorize]
    public class LeavesController : Controller
    {
        private readonly ILeaveService leaveService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly ITeamService teamService;
        private readonly IEmployeeLeaveTypesService employeeLeaveTypesService;
        private readonly IEmployeeService employeeService;

        private readonly UserManager<Employee> userManager;

        //private readonly LeaveSystemDbContext db; //ToDo: Later maybe the db should be removed


        public LeavesController(
            ILeaveService leaveService,
            ITeamService teamService,
            ILeaveTypeService leaveTypeService,
            UserManager<Employee> userManager,
            IEmployeeLeaveTypesService employeeLeaveTypesService,
            IEmployeeService employeeService)
        {
            this.leaveService = leaveService;
            this.teamService = teamService;
            this.leaveTypeService = leaveTypeService;
            this.userManager = userManager;
            this.employeeLeaveTypesService = employeeLeaveTypesService;
            this.employeeService = employeeService;
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

            var businessDaysCount = GetBusinessDays(leaveModel.StartDate, leaveModel.EndDate);

            if (leaveModel.TotalDays != businessDaysCount || leaveModel.TotalDays == 0)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "Count of days is not correct or it is equal to zero.");
            }

            var leaveTypeExist = this.leaveTypeService.TypeExist(leaveModel.LeaveTypeId);

            if (!leaveTypeExist)
            {
                this.ModelState.AddModelError(nameof(leaveModel.LeaveTypeId), "Leave type does not exist.");
            }

            var teamId = this.userManager.FindByIdAsync(this.User.GetId()).GetAwaiter().GetResult().TeamId;

            var employeeExist = this.teamService.EmployeeExistInTeam(teamId, this.User.GetId());
            if (!employeeExist) //ToDo: Change it with current user teamId
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

            //employeeLeave.PendingApprovalDays = employeeLeave.PendingApprovalDays + leaveModel.TotalDays;

            var approveEmployeeId = this.employeeService.GetTeamLeadId(this.User.GetId());

            this.leaveService.Create(
                leaveModel.StartDate.Date,
                leaveModel.EndDate.Date,
                leaveModel.TotalDays,
                leaveModel.LeaveTypeId,
                this.User.GetId(),
                leaveModel.SubstituteEmployeeId,
                approveEmployeeId,
                leaveModel.Comments,
                leaveModel.RequestDate
                );

            if (this.User.IsUser())
            {
                return RedirectToAction("History", "Statistic");
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

            return View(new LeaveFormModel
            {
                Id= leave.Id,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                TotalDays = leave.TotalDays,
                RequestEmployeeId = leave.RequestEmployeeId,
                SubstituteEmployeeId = leave.SubstituteEmployeeId,
                ApproveEmployeeId = leave.ApproveEmployeeId ?? leave.RequestEmployeeId,
                Comments = leave.Comments,
                LeaveTypeId = leave.LeaveTypeId,
                LeaveTypes = leave.LeaveTypes,
                EmployeesInTeam = leave.EmployeesInTeam,

            });
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

            var businessDaysCount = GetBusinessDays(leaveModel.StartDate, leaveModel.EndDate);

            if (leaveModel.TotalDays != businessDaysCount || leaveModel.TotalDays == 0)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "Count of days is not correct or it is equal to zero.");
            }

            var leaveTypeExist = this.leaveTypeService.TypeExist(leaveModel.LeaveTypeId);

            if (!leaveTypeExist)
            {
                this.ModelState.AddModelError(nameof(leaveModel.LeaveTypeId), "Leave type does not exist.");
            }


           
            var teamId = this.userManager.FindByIdAsync(leaveModel.RequestEmployeeId).GetAwaiter().GetResult().TeamId;

            var employeeExist = this.teamService.EmployeeExistInTeam(teamId, this.User.GetId());
            if (!this.User.IsAdmin() && !employeeExist) //ToDo: Change it with current user teamId
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
               leaveModel.Comments
               );

            if (this.User.IsInRole(UserRoleName))
            {
                return RedirectToAction("History", "Statistic");
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

            return RedirectToAction(nameof(ForApproval));
        }

        private static double GetBusinessDays(DateTime startDate, DateTime endDate)
        {
            double calcBusinessDays =
                1 + ((endDate - startDate).TotalDays * 5 -
                (startDate.DayOfWeek - endDate.DayOfWeek) * 2) / 7;

            if (endDate.DayOfWeek == DayOfWeek.Saturday)
            {
                calcBusinessDays--;
            }

            if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                calcBusinessDays--;
            }

            return calcBusinessDays;
        }

        private static bool IsInRange(DateTime currentDate, DateTime startDate, DateTime endDate)
        {
            return currentDate >= startDate && currentDate <= endDate;
        }

    }

}
