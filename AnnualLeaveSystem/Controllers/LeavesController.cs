namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services;
    using Microsoft.AspNetCore.Mvc;

    public class LeavesController : Controller
    {
        private readonly IGetLeaveTypesService getLeaveTypesService;
        private readonly IGetEmployeesInTeamService getEmployeesInTeamService;

        public LeavesController(
            IGetLeaveTypesService getLeaveTypesService,
            IGetEmployeesInTeamService getEmployeesInTeamService)
        {
            this.getLeaveTypesService = getLeaveTypesService;
            this.getEmployeesInTeamService = getEmployeesInTeamService;
        }

        public IActionResult Add()
        {
            var model = new AddLeaveFormModel
            {
                LeaveTypes = this.getLeaveTypesService.GetLeaveTypes(),
                EmployeesInTeam = this.getEmployeesInTeamService.GetEmployeesInTeam(),
            };
            return View(model);
        }

        //var viewModel = new CreateLeaveRequestInputModel
        //{
        //    LeaveTypes = this.leaveTypeService.GetAllLeaveTypes(),
        //    EmployeesInTeam = this.employeesInTeamService.GetAllEmployeesInTeam(),
        //};
        //return this.View(viewModel);


        [HttpPost]
        public IActionResult Add(AddLeaveFormModel leaveModel)
        {
            return null;


        }
    }
}
