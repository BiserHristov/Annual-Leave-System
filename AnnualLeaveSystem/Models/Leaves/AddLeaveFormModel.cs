namespace AnnualLeaveSystem.Models.Leaves
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddLeaveFormModel
    {
        [Display(Name ="Start date")]
        public DateTime StartDate { get; init; }

        [Display(Name = "End date")]
        public DateTime EndDate { get; init; }

        [Display(Name = "Leave type")]
        public int LeaveTypeId { get; init; }

        public string ReplacementEmployeeId { get; init; }

        public string Comments { get; init; }

        public IEnumerable<LeaveTypeViewModel> LeaveTypes { get; set; }

        public IEnumerable<ReplacementEmployeeViewModel> EmployeesInTeam { get; set; }
    }
}
