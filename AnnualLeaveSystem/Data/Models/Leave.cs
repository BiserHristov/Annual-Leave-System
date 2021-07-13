namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Leave
    {
        public int Id { get; init; }

        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        public LeaveType LeaveType { get; set; }

        [Required]
        public int RequestEmployeeId { get; set; }

        public Employee RequestEmployee { get; set; }

        [Required]
        public int SubstituteEmployeeId { get; set; }

        public Employee SubstituteEmployee { get; set; }

        [Required]
        public int ApproveEmployeeId { get; set; }

        public Employee ApproveEmployee { get; set; }

        public bool IsApproved { get; set; } = false;

        public bool IsCancelled { get; set; } = false;

        public string Comments { get; set; }
    }
}
