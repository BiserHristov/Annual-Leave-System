namespace AnnualLeaveSystem.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Models.Users;
    using AnnualLeaveSystem.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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
            if ((this.User.Identity.IsAuthenticated && !this.User.IsAdmin()) || this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new EmployeeFormModel();

            this.AddTeamsAndDepartments(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(EmployeeFormModel user)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(user.Password), "Password and confirm password does not match.");
                ModelState.AddModelError(nameof(user.ConfirmPassword), "Password and confirm password does not match.");
            }

            if (this.userService.Exist(user.Email))
            {
                ModelState.AddModelError(nameof(user.Email), "Email is already taken.");
            }

            if (!ModelState.IsValid)
            {
                AddTeamsAndDepartments(user);
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

                AddTeamsAndDepartments(user);

                return View(user);
            }

            userService.AddLeaveTypesToEmployee(registeredUser.Id);

            await signInManager.SignInAsync(registeredUser, true);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel user)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var searchedUser = await this.userManager.FindByEmailAsync(user.Email);

            if (searchedUser == null)
            {
                return AddError(user);
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(searchedUser, user.Password);

            if (!passwordIsValid)
            {
                return AddError(user);
            }

            await signInManager.SignInAsync(searchedUser, true);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private void AddTeamsAndDepartments(EmployeeFormModel user)
        {
            user.Teams = this.userService.AllTeams();
            user.Departments = this.userService.AllDepartments();
        }

        private IActionResult AddError(LoginFormModel user)
        {
            const string InvalidCredentialsMessage = "Credentials are not valid";
            ModelState.AddModelError(string.Empty, InvalidCredentialsMessage);

            return View(user);
        }
    }
}
