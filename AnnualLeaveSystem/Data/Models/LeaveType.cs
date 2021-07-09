namespace AnnualLeaveSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants;
    public class LeaveType
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(LeaveTypeNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public int DefaultDays { get; set; }

        public ICollection<Leave> Leaves { get; init; } = new HashSet<Leave>();
    }
}
