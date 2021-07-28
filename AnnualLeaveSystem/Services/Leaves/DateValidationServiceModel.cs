namespace AnnualLeaveSystem.Services.Leaves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DateValidationServiceModel
    {
        public int Id{get;init;}
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

    }
}
