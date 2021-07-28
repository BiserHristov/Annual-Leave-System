namespace AnnualLeaveSystem.Models.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Services.Leaves;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllLeavesQueryModel
    {
        public const int LeavesPerPage = 2;
        public int CurrentPage { get; set; } = 1;
        public int TotalLeaves { get; set; }
        public bool HasPreviousPage => this.CurrentPage > 1;
        public int PreviousPageNumber => this.CurrentPage - 1;
        public int MaxPage => (int)Math.Ceiling((double)this.TotalLeaves / LeavesPerPage);
        public bool HasNextPage => this.CurrentPage < this.MaxPage;
        public int NextPageNumber => this.CurrentPage + 1;

        public Status? Status { get; init; }
        public IEnumerable<Status> Statuses { get; set; } = new HashSet<Status>();

        [Display(Name = "First Name")]
        public string FirstName { get; init; }

        [Display(Name = "Last Name")]
        public string LastName { get; init; }


        [Display(Name = "Sort by")]
        public LeaveSorting Sorting { get; set; }
        // public IEnumerable<LeaveSorting> Sortings { get; init; } = new HashSet<LeaveSorting>();

        public IEnumerable<LeaveServiceModel> Leaves { get; set; } = new HashSet<LeaveServiceModel>();
    }
}
