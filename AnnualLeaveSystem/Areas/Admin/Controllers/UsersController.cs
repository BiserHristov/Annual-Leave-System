namespace AnnualLeaveSystem.Areas.Admin.Controllers
{
    using AnnualLeaveSystem.Areas.Admin.Services.Departments;
    using AnnualLeaveSystem.Areas.Admin.Services.Employees;
    using AnnualLeaveSystem.Areas.Admin.Services.Teams;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Models.Users;
    using AnnualLeaveSystem.Services.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using static AnnualLeaveSystem.WebConstants;



    public class UsersController : BaseAdminController
    {
        private readonly IEmployeeService employeeService;
        private readonly IUserService userService;
        private readonly IDepartmentService departmentService;
        private readonly ITeamService teamService;
        private readonly UserManager<Employee> userManager;
        private readonly RoleManager<IdentityRole> rolemanager;


        public UsersController(IEmployeeService employeeService,
            IDepartmentService departmentService,
            ITeamService teamService, IUserService userService,
            UserManager<Employee> userManager,
            RoleManager<IdentityRole> rolemanager)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.teamService = teamService;
            this.userService = userService;
            this.userManager = userManager;
            this.rolemanager = rolemanager;
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

            employee.MiddleName = string.IsNullOrEmpty(employee.MiddleName) ? "-" : employee.MiddleName;
            employee.Departments = this.departmentService.All();
            employee.Teams = this.teamService.All();

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

            if (!departments.Any(d => d.Id == model.DepartmentId))
            {
                ModelState.AddModelError(nameof(model.DepartmentId), "Department Id is not valid.");
            }

            if (model.HireDate.Year < DateTime.Now.AddYears(-100).Year ||
                model.HireDate > DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.HireDate), "Date is not valid.");
            }

            if (!ModelState.IsValid)
            {
                model.Departments = this.departmentService.All();
                model.Teams = this.teamService.All();
                return View(model);
            }

            this.employeeService.Edit(model);


            return RedirectToAction(nameof(All));
        }


        public IActionResult Add()
        {
            if (this.User.Identity.IsAuthenticated && !this.User.IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }


            var model = new EmployeeFormModel();
            model.Teams = this.userService.AllTeams();
            model.Departments = this.userService.AllDepartments();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeFormModel model)
        {
            if (!this.User.IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(model.Password), "Password and confirm password does not match!");
                ModelState.AddModelError(nameof(model.ConfirmPassword), "Password and confirm password does not match!");
            }

            if (!ModelState.IsValid)
            {
                model.Teams = this.userService.AllTeams();
                model.Departments = this.userService.AllDepartments();
                return View(model);
            }

            var teamLeadId = userService.GetTeamLeadId(model.TeamId);

            var registeredUser = new Employee
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                ImageUrl = model.ImageUrl,
                JobTitle = model.JobTitle,
                DepartmentId = model.DepartmentId,
                TeamId = model.TeamId,
                TeamLeadId = teamLeadId,
                HireDate = model.HireDate.ToUniversalTime().Date

            };

            var result = await this.userManager.CreateAsync(registeredUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);

                }

                model.Teams = this.userService.AllTeams();
                model.Departments = this.userService.AllDepartments();

                return View(model);
            }

            userService.AddLeaveTypesToEmployee(registeredUser.Id);

            return RedirectToAction("All", "Users");

        }

       
        public IActionResult Delete (string Id)
        {
            if (!this.User.IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var result = this.employeeService.Delete(Id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Users");
        }
    }
}
