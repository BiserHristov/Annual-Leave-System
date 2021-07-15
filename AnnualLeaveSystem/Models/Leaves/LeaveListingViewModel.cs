namespace AnnualLeaveSystem.Models.Leaves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LeaveListingViewModel
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string StartDate { get; init; }
        public string EndDate { get; init; }
        public int TotalDays { get; init; }
        public string Status { get; init; }
        public string RequestDate { get; init; }


    }
}
