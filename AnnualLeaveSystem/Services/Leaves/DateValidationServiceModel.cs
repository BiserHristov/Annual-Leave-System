namespace AnnualLeaveSystem.Services.Leaves
{
    using System;

    public class DateValidationServiceModel
    {
        public int Id{get;init;}
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

    }
}
