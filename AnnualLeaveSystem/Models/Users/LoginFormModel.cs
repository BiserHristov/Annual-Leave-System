namespace AnnualLeaveSystem.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    public class LoginFormModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        //[StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; init; }
    }
}
