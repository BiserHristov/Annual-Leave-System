namespace AnnualLeaveSystem.Models.Users
{
    using AnnualLeaveSystem.Services.Users;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
    public class RegisterFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; init; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string ConfirmPassword { get; init; }

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        public string JobTitle { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public string TeamLeadId { get; set; }

        public int TeamId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public IEnumerable<RegisterDepartamentViewModel> Departments { get; set; } = new HashSet<RegisterDepartamentViewModel>();


        public IEnumerable<int> Teams { get; set; } = new HashSet<int>();


    }
}
