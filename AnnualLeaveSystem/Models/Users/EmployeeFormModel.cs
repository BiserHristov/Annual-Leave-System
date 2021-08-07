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

        //[Required]
        //[StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        //public string FirstName { get; set; }

        //[StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength)]
        //public string MiddleName { get; set; }

        //[Required]
        //[StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        //public string LastName { get; set; }

        //[Required]
        //[Url]
        //public string ImageUrl { get; set; }

        //[Required]
        //[StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        //public string JobTitle { get; set; }

        //[Required]
        //public int DepartmentId { get; set; }


        //public int TeamId { get; set; }

        //[Required]
        //public DateTime HireDate { get; set; }

        //public IEnumerable<RegisterDepartamentServiceModel> Departments { get; set; } = new HashSet<RegisterDepartamentServiceModel>();


        //public IEnumerable<int> Teams { get; set; } = new HashSet<int>();


    }
}
