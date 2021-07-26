namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetOfficialHolidaysService : IGetOfficialHolidaysService
    {
        private readonly LeaveSystemDbContext db;

        public GetOfficialHolidaysService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<OfficialHoliday> GetHolidays()
        {
            return this.db.OfficialHolidays.ToList();
        }
    }
}
