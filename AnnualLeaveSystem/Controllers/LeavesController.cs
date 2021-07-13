namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using static AnnualLeaveSystem.Data.DataConstants;
    public class LeavesController : Controller
    {
        private readonly IGetLeaveTypesService getLeaveTypesService;
        private readonly IGetEmployeesInTeamService getEmployeesInTeamService;
        private readonly IGetOfficialHolidaysService getOfficialHolidaysService;
        private readonly LeaveSystemDbContext db; //ToDo: Later maybe the db should be removed
        public LeavesController(
            IGetLeaveTypesService getLeaveTypesService,
            IGetEmployeesInTeamService getEmployeesInTeamService,
            IGetOfficialHolidaysService getOfficialHolidaysService,
            LeaveSystemDbContext db)
        {
            this.getLeaveTypesService = getLeaveTypesService;
            this.getEmployeesInTeamService = getEmployeesInTeamService;
            this.getOfficialHolidaysService = getOfficialHolidaysService;
            this.db = db;
        }

        public IActionResult Add()
        {
            var model = new AddLeaveFormModel
            {
                LeaveTypes = this.getLeaveTypesService.GetLeaveTypes(),
                EmployeesInTeam = this.getEmployeesInTeamService.GetEmployeesInTeam(),
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

            if (!this.db.Teams.Any(t => t.Id == 4 && t.Employees.Any(e => e.Id == leaveModel.SubstituteEmployeeId))) //ToDo: Change it with current user teamId
            {
                this.ModelState.AddModelError(nameof(leaveModel.SubstituteEmployeeId), "There is no such employee in your team.");
            }

            var leaveType = this.db.LeaveTypes.FirstOrDefault(lt => lt.Id == leaveModel.LeaveTypeId); //ToDo: Magic??!?!

            var employeeLeave = this.db.EmployeesLeaveTypes
                .Where(el => el.EmployeeId == _EmployeeId &&
                       el.LeaveTypeId == leaveModel.LeaveTypeId)
                .FirstOrDefault(); //ToDo: Change it with current user Id

            if (employeeLeave.RemainingDays == 0 || employeeLeave.RemainingDays < leaveModel.TotalDays)
            {
                this.ModelState.AddModelError(nameof(leaveModel.TotalDays), "You do no have enough days left from the selected leave type option.");
            }


            var leaves = this.db.Leaves.Where(l => l.RequestEmployeeId == _EmployeeId && l.EndDate >= DateTime.UtcNow.Date).ToList();  //ToDo: Change it with current user Id

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
                .Where(l => l.SubstituteEmployeeId == _EmployeeId &&
                            l.IsApproved &&
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
                leaveModel.EmployeesInTeam = this.getEmployeesInTeamService.GetEmployeesInTeam();
                return View(leaveModel);
            }

            employeeLeave.UsedDays = employeeLeave.UsedDays + leaveModel.TotalDays;
            var approveEmployeeId = db.Employees.Where(e => e.Id == _EmployeeId).Select(e => e.TeamLeadId).FirstOrDefault();

            var leave = new Leave
            {
                StartDate = leaveModel.StartDate.Date,
                EndDate = leaveModel.EndDate.Date,
                LeaveTypeId = leaveModel.LeaveTypeId,
                RequestEmployeeId = _EmployeeId, //ToDo: Change it with current user Id
                SubstituteEmployeeId = leaveModel.SubstituteEmployeeId,
                ApproveEmployeeId = approveEmployeeId, //ToDo: Change it with approveEmployeeId
                Comments = leaveModel.Comments,
            };

            this.db.Leaves.Add(leave);
            this.db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        //public IActionResult Preview(AddLeaveFormModel leaveModel)
        //{
        //    return View(leaveModel);
        //    // return RedirectToAction("Index", "Home");
        //}
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
