namespace AnnualLeaveSystem.Models.Users
{
    using AnnualLeaveSystem.Areas.Admin.Services.Employees;
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
    public class EmployeeFormModel : EditEmployeeServiceModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; init; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; init; }

    }
}
