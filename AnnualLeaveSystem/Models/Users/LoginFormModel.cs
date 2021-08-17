namespace AnnualLeaveSystem.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
    public class LoginFormModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; init; }
    }
}
