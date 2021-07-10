namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGetOfficialHolidaysService
    {
        public IEnumerable<OfficialHoliday> GetHolidays();
    }
}
