namespace AnnualLeaveSystem.Services.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using System.Collections.Generic;

    public class LeaveQueryServiceModel
    {
        public Status? Status { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public LeaveSorting Sorting { get; init; }

        public int CurrentPage { get; init; }

        public int LeavesPerPage { get; init; }

        public int TotalLeaves { get; init; }

        public IEnumerable<LeaveServiceModel> Leaves { get; init; }



    }
}
