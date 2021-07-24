namespace AnnualLeaveSystem.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
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
