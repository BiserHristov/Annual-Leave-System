namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Users;
    using AnnualLeaveSystem.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;
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
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterFormModel();

            AddTeamsAndDepartments(model);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel user)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(user.Password), "Password and confirm password does not match!");
                ModelState.AddModelError(nameof(user.ConfirmPassword), "Password and confirm password does not match!");
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

            await userManager.AddClaimAsync(registeredUser, new Claim("TeamId",user.TeamId.ToString()));

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
        private void AddTeamsAndDepartments(RegisterFormModel user)
        {
            user.Teams = this.userService.AllTeams();
            user.Departments = this.userService.AllDepartments();
        }
        private IActionResult AddError(LoginFormModel user)
        {
            const string invalidCredentialsMessage = "Credentials are not valid";
            ModelState.AddModelError(string.Empty, invalidCredentialsMessage);

            return View(user);

        }



    }
}
