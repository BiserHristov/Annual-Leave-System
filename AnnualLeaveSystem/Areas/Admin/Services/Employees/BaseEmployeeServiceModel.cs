namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
    public abstract class BaseEmployeeServiceModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength)]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }


        [Required]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        [Display(Name = "Job title")]
        public string JobTitle { get; set; }

        [Display(Name = "Team ID")]
        public int? TeamId { get; set; }

        [Required]
        [Display(Name = "Hire date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
    }

}

