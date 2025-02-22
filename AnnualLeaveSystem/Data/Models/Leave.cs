﻿namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Leave
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; init; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime RequestDate { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        public LeaveType LeaveType { get; set; }

        [Required]
        public string RequestEmployeeId { get; set; }

        public Employee RequestEmployee { get; set; }

        [Required]
        public string SubstituteEmployeeId { get; set; }

        public Employee SubstituteEmployee { get; set; }

        public bool ApprovedBySubstitute { get; set; } = false;

        public string ApproveEmployeeId { get; set; }

        public Employee ApproveEmployee { get; set; }

        [EnumDataType(typeof(Status))]
        public Status LeaveStatus { get; set; } = Status.Pending;

        public string Comments { get; set; }
    }
}
