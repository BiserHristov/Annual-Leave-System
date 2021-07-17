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
        public const int LeavesPerPage = 2;
        public int CurrentPage { get; set; } = 1;

        [Display(Name = "First Name")]
        public string FirstName { get; init; }

        [Display(Name = "Last Name")]
        public string LastName { get; init; }

        public Status? Status { get; init; }

        public int TotalLeaves { get; set; }

        public IEnumerable<Status> Statuses { get; set; } = new HashSet<Status>();

        [Display(Name = "Sort by")]
        public LeaveSorting Sorting { get; set; }
        // public IEnumerable<LeaveSorting> Sortings { get; init; } = new HashSet<LeaveSorting>();

        public IEnumerable<LeaveListingViewModel> Leaves { get; set; } = new HashSet<LeaveListingViewModel>();
    }
}
