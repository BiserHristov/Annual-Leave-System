namespace AnnualLeaveSystem.Models.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class AllLeavesQueryModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; init; }

        [Display(Name = "Last Name")]
        public string LastName { get; init; }

        [Display(Name = "Start date")]
        public string StartDate { get; init; }

        public string Status { get; init; }

        public IEnumerable<Status> Statuses { get; init; } = new HashSet<Status>();

        [Display(Name = "Sort by")]
        public LeaveSorting Sorting { get; set; }

        public IEnumerable<LeaveListingViewModel> Leaves { get; init; } = new HashSet<LeaveListingViewModel>();
    }
}
