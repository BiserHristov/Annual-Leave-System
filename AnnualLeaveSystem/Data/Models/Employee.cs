namespace AnnualLeaveSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static AnnualLeaveSystem.Data.DataConstants.Employee;
    public class Employee : IdentityUser
    {
        
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(MiddleNameMaxLength)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public string TeamLeadId { get; set; }

        public Employee TeamLead { get; set; }

       // [Required]
        public int? TeamId { get; set; }

        public Team Team { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime HireDate { get; set; }

        [InverseProperty("RequestEmployee")]
        public virtual ICollection<Leave> RequestedLeaves { get; init; } = new HashSet<Leave>();

        [InverseProperty("SubstituteEmployee")]
        public virtual ICollection<Leave> SubstituteLeaves { get; init; } = new HashSet<Leave>();

        [InverseProperty("ApproveEmployee")]
        public virtual ICollection<Leave> ApprovedLeaves { get; init; } = new HashSet<Leave>();

        public virtual ICollection<EmployeeLeaveType> EmployeesTypes { get; set; } = new HashSet<EmployeeLeaveType>();

    }
}
