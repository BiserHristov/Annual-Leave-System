namespace AnnualLeaveSystem.Services.Holidays
{
    using AnnualLeaveSystem.Data;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HolidayService : IHolidayService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;

        public HolidayService(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<HolidayServiceModel> All()
        {
            return this.db.OfficialHolidays
                 .ProjectTo<HolidayServiceModel>(this.mapper)
                 .ToList();
        }

        public IEnumerable<string> AllDates()
        {
            return this.db.OfficialHolidays
                  .Select(h => h.Date.ToLocalTime().Date.ToString("dd.MM.yyyy"))
                  .ToList();
        }

        public (bool, string) IsHoliday(DateTime date)
        {
            var holiday = this.db.OfficialHolidays
                .Where(h => h.Date == date.Date)
                .FirstOrDefault();

            if (holiday == null)
            {
                return (false, string.Empty);
            }

            return (true, holiday.Name);
        }

    }
}
