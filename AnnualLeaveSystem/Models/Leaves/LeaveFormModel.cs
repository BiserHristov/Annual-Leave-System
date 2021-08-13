namespace AnnualLeaveSystem.Models.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Services.Leaves;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LeaveFormModel
    {
        public int Id  { get; init;}

        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Leave type")]
        public int LeaveTypeId { get; init; }

        public string RequestEmployeeId { get; init; }

        [Required]
        [Display(Name = "Substitute employee")]
        public string SubstituteEmployeeId { get; init; }

        public string ApproveEmployeeId { get; set; }

        [Display(Name = "Total days")]
        [Range(1, int.MaxValue, ErrorMessage = "Your leave days count should be bigger than 0")]
        public int TotalDays { get; set; }

        public string Comments { get; init; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow.Date;

        public IEnumerable<LeaveTypeServiceModel> LeaveTypes { get; set; }

        public IEnumerable<OfficialHoliday> ОfficialHolidays { get; set; }

        public IEnumerable<SubstituteEmployeeServiceModel> EmployeesInTeam { get; set; }
    }
}
