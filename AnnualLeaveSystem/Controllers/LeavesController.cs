namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

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
            if (!this.db.LeaveTypes.Any(lt => lt.Id == leaveModel.LeaveTypeId))
            {
                this.ModelState.AddModelError(nameof(leaveModel.LeaveTypeId), "Leave type does not exist.");
            }

            if (!this.db.Teams.Any(t => t.Id == 4 && t.Employees.Any(e => e.Id == leaveModel.ReplacementEmployeeId))) //ToDo: Change it with current user teamId
            {
                this.ModelState.AddModelError(nameof(leaveModel.ReplacementEmployeeId), "There is no such employee in your team.");
            }

            if (leaveModel.StartDate > leaveModel.EndDate)
            {
                this.ModelState.AddModelError(nameof(leaveModel.StartDate), "Start date should be before end date.");
                this.ModelState.AddModelError(nameof(leaveModel.EndDate), "End date should be after start date.");

            }

            if (!ModelState.IsValid)
            {
                leaveModel.LeaveTypes = this.getLeaveTypesService.GetLeaveTypes();
                leaveModel.EmployeesInTeam = this.getEmployeesInTeamService.GetEmployeesInTeam();
                return View(leaveModel);
            }




            return RedirectToAction("Index", "Home");
        }
    }
}
