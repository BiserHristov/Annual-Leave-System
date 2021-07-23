namespace AnnualLeaveSystem.Services.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IStatisticsService
    {
        public StatisticsServiceModel Get();
    }
}
