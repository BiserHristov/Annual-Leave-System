namespace AnnualLeaveSystem.Services.Leaves
{
    using System;

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
