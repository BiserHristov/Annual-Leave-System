namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Users;
    using AnnualLeaveSystem.Services.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<Employee> userManager;
        private readonly SignInManager<Employee> signInManager;

        public UsersController(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
        }

        public IActionResult Register()
        {
            var model = new RegisterFormModel
            {
                Teams = this.userService.AllTeams(),
                Departments = this.userService.AllDepartments(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel user)
        {

            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(user.Password), "Password and confirm password does not match!");
                ModelState.AddModelError(nameof(user.ConfirmPassword), "Password and confirm password does not match!");
            }

            if (!ModelState.IsValid)
            {
                user.Teams = this.userService.AllTeams();
                user.Departments = this.userService.AllDepartments();
                return View(user);
            }
            var teamLeadId = userService.GetTeamLeadId(user.TeamId);
            var registeredUser = new Employee
            {
                Email = user.Email,
                UserName = user.Email,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                JobTitle = user.JobTitle,
                DepartmentId = user.DepartmentId,
                TeamId = user.TeamId,
                TeamLeadId = teamLeadId,
                HireDate = user.HireDate.ToUniversalTime().Date

            };

            if (string.IsNullOrWhiteSpace(teamLeadId))
            {
                registeredUser.TeamLeadId = registeredUser.Id;
            }

            var result = await this.userManager.CreateAsync(registeredUser, user.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);

                }

                return View(user);
            }


            return RedirectToAction("Index", "Home");
        }
    }
}
