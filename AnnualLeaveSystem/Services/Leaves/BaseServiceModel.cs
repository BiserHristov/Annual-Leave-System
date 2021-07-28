namespace AnnualLeaveSystem.Services.Leaves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class BaseServiceModel
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string RequestEmployeeId { get; init; }
        public string SubstituteEmployeeId { get; init; }
        public bool ApprovedBySubstitute { get; set; }
        public int TotalDays { get; init; }

    }
}
