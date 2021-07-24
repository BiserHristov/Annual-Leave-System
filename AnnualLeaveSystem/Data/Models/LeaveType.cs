namespace AnnualLeaveSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants.LeaveType;
    public class LeaveType
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public int DefaultDays { get; set; }

        public ICollection<Leave> Leaves { get; init; } = new HashSet<Leave>();
        public ICollection<EmployeeLeaveType> TypesEmployees { get; set; } = new HashSet<EmployeeLeaveType>();
    }
}
