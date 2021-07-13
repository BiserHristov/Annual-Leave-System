namespace AnnualLeaveSystem.Models.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddLeaveFormModel
    {
        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Leave type")]
        public int LeaveTypeId { get; init; }

        [Required]
        public int SubstituteEmployeeId { get; init; }

        public int RequestEmployeeId { get; init; }
        public int TotalDays { get; set; }

        public string Comments { get; init; }

        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;

        public IEnumerable<LeaveTypeViewModel> LeaveTypes { get; set; }

        public IEnumerable<OfficialHoliday> ОfficialHolidays { get; set; }

        public IEnumerable<ReplacementEmployeeViewModel> EmployeesInTeam { get; set; }
    }
}
