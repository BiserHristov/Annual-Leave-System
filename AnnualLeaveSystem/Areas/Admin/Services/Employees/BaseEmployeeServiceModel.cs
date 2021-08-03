namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
    public abstract class BaseEmployeeServiceModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [MaxLength(MiddleNameMaxLength)]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }


        [Required]
        [MaxLength(JobTitleMaxLength)]
        [Display(Name = "Job title")]
        public string JobTitle { get; set; }

        [Display(Name = "Team ID")]
        public int? TeamId { get; set; }

        [Display(Name = "Hire date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
    }

}

