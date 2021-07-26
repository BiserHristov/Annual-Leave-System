namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services;
    using AnnualLeaveSystem.Services.Leaves;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static AnnualLeaveSystem.Data.DataConstants.User;

    [Authorize]
    public class LeavesController : Controller
    {
        private readonly ILeaveService leaveService;

        private readonly IGetLeaveTypesService getLeaveTypesService;
        private readonly IGetEmployeesInTeamService getEmployeesInTeamService;
        private readonly IGetOfficialHolidaysService getOfficialHolidaysService;
        private readonly LeaveSystemDbContext db; //ToDo: Later maybe the db should be removed


        public LeavesController(
            IGetLeaveTypesService getLeaveTypesService,
            IGetEmployeesInTeamService getEmployeesInTeamService,
            IGetOfficialHolidaysService getOfficialHolidaysService,
            LeaveSystemDbContext db, ILeaveService leaveService)
        {

            this.getLeaveTypesService = getLeaveTypesService;
            this.getEmployeesInTeamService = getEmployeesInTeamService;
            this.getOfficialHolidaysService = getOfficialHolidaysService;
            this.db = db;
            this.leaveService = leaveService;
        }

        public IActionResult Add()
        {
            var model = new AddLeaveFormModel
            {
                LeaveTypes = this.getLeaveTypesService.GetLeaveTypes(),
                EmployeesInTeam = this.getEmployeesInTeamService.GetEmployeesInTeam(this.User.GetId()),
                ОfficialHolidays = this.getOfficialHolidaysService.GetHolidays(),
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Add(AddLeaveFormModel leaveModel)
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

            if (!this.db.LeaveTypes.Any(lt => lt.Id == leaveModel.LeaveTypeId))
            {
                this.ModelState.AddModelError(nameof(leaveModel.LeaveTypeId), "Leave type does not exist.");
            }


            if (!this.db.Teams.Any(t => t.Id == _EmployeeTeamId && t.Employees.Any(e => e.Id == leaveModel.SubstituteEmployeeId))) //ToDo: Change it with current user teamId
            {
                this.ModelState.AddModelError(nameof(leaveModel.SubstituteEmployeeId), "There is no such employee in your team.");
            }


            var employeeLeave = this.db.EmployeesLeaveTypes
                .Include(x => x.LeaveType)
                .Where(el => el.EmployeeId == this.User.GetId() &&
                       el.LeaveTypeId == leaveModel.LeaveTypeId)
                .FirstOrDefault(); //ToDo: Change it with current user Id

            if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays < leaveModel.TotalDays)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "You do no have enough days left from the selected leave type option.");
            }


            var leaves = this.db.Leaves.Where(l => l.RequestEmployeeId == this.User.GetId() && l.EndDate >= DateTime.UtcNow.Date).ToList();  //ToDo: Change it with current user Id

            for (int i = 0; i < leaves.Count; i++)
            {
                var currentLeave = leaves[i];

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

            var substituteLeaves = this.db.Leaves
                .Where(l => l.SubstituteEmployeeId == this.User.GetId() &&
                            l.LeaveStatus == Status.Approved &&
                            l.EndDate >= DateTime.UtcNow.Date)
                .ToList();  //ToDo: Change it with current user Id

            for (int i = 0; i < substituteLeaves.Count; i++)
            {
                var currentLeave = substituteLeaves[i];

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
                leaveModel.LeaveTypes = this.getLeaveTypesService.GetLeaveTypes();
                leaveModel.EmployeesInTeam = this.getEmployeesInTeamService.GetEmployeesInTeam(this.User.GetId());
                return View(leaveModel);
            }

            employeeLeave.UsedDays = employeeLeave.UsedDays + leaveModel.TotalDays;
            var approveEmployeeId = db.Employees.Where(e => e.Id == this.User.GetId()).Select(e => e.TeamLeadId).FirstOrDefault();

            var leave = new Leave
            {
                StartDate = leaveModel.StartDate.Date,
                EndDate = leaveModel.EndDate.Date,
                TotalDays = leaveModel.TotalDays,
                LeaveTypeId = leaveModel.LeaveTypeId,
                RequestEmployeeId = this.User.GetId(), //ToDo: Change it with current user Id
                SubstituteEmployeeId = leaveModel.SubstituteEmployeeId,
                ApproveEmployeeId = approveEmployeeId, //ToDo: Change it with approveEmployeeId
                Comments = leaveModel.Comments,
                RequestDate = leaveModel.RequestedDate
            };

            this.db.Leaves.Add(leave);
            this.db.SaveChanges();

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
                AllLeavesQueryModel.LeavesPerPage);


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

            var leave = this.db.Leaves
                 .Include(l => l.LeaveType)
                .Include(l => l.RequestEmployee)
                .Include(l => l.ApproveEmployee)
                .Include(l => l.SubstituteEmployee)
                .FirstOrDefault(l => l.Id == leaveId);

            if (leave == null)
            {
                return NotFound();
            }

            return View(new LeaveDetailsViewModel
            {
                RequestEmployeeName = leave.RequestEmployee.FirstName + " " + leave.RequestEmployee.MiddleName + " " + leave.RequestEmployee.LastName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                TotalDays = leave.TotalDays,
                Type = leave.LeaveType.Name,
                Status = leave.LeaveStatus.ToString(),
                ApproveEmployeeName = leave.ApproveEmployee.FirstName + " " + leave.ApproveEmployee.MiddleName + " " + leave.ApproveEmployee.LastName,
                SubstituteEmployeeName = leave.SubstituteEmployee.FirstName + " " + leave.SubstituteEmployee.MiddleName + " " + leave.SubstituteEmployee.LastName,
                RequestDate = leave.RequestDate,
                Comments = leave.Comments
            });
        }

        private static double GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday)
            {
                calcBusinessDays--;
            }

            if (startD.DayOfWeek == DayOfWeek.Sunday)
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
