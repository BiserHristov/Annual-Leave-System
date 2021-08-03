namespace AnnualLeaveSystem.Areas.Admin.Controllers
{
    using AnnualLeaveSystem.Areas.Admin.Services.Departments;
    using AnnualLeaveSystem.Areas.Admin.Services.Employees;
    using AnnualLeaveSystem.Areas.Admin.Services.Teams;
    using AnnualLeaveSystem.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class UsersController : BaseAdminController
    {
        private readonly IEmployeeService employeeService;

        private readonly IDepartmentService departmentService;
        private readonly ITeamService teamService;


        public UsersController(IEmployeeService employeeService,
            IDepartmentService departmentService,
            ITeamService teamService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.teamService = teamService;
        }

        public IActionResult All()
        {
            var employees = this.employeeService.AllEmployees();
            return View(employees);
        }
        public IActionResult Edit(string Id)
        {
            var employee = this.employeeService.GetEmployee(Id);

            if (employee == null || !this.User.IsAdmin())
            {
                return BadRequest();
            }

            employee.DepartmentIDs = this.departmentService.All();
            employee.TeamIDs = this.teamService.All();

            return View(employee);

        }

        [HttpPost]
        public IActionResult Edit(EditEmployeeServiceModel model)
        {
            if (!this.User.IsAdmin())
            {
                return BadRequest();
            }

            var employee = this.employeeService.GetEmployee(model.Id);

            if (employee == null)
            {
                return BadRequest();
            }

            var teams = this.teamService.All().ToList();

            if (model.TeamId != null && !teams.Contains(model.TeamId ?? -1))
            {
                ModelState.AddModelError(nameof(model.TeamId), "Team Id is not valid.");
            }

            var departments = this.departmentService.All().ToList();

            if (!departments.Contains(model.DepartmentId))
            {
                ModelState.AddModelError(nameof(model.DepartmentId), "Department Id is not valid.");
            }

            if (model.HireDate.Year< DateTime.Now.AddYears(-100).Year ||
                model.HireDate>DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.HireDate), "Date is not valid.");
            }

            if (!ModelState.IsValid)
            {
                model.DepartmentIDs = this.departmentService.All();
                model.TeamIDs = this.teamService.All();
                return View(model);
            }

            this.employeeService.Edit(model);


            return RedirectToAction(nameof(All));
        }
    }
}
