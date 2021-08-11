namespace AnnualLeaveSystem.Services.Holidays
{
    using AnnualLeaveSystem.Data;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HolidayService : IHolidayService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;
        private readonly IMemoryCache cache;
        private const string AllHolidayDatesCacheKey = "AllHolidayDates";
        private const string AllHolidaysCacheKey = "AllHolidays";
        public HolidayService(LeaveSystemDbContext db, IMapper mapper, IMemoryCache cache)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
            this.cache = cache;
        }
        //const string AllHolidayDatesCacheKey = "AllHolidayDates";

        //var allHolidays = this.cache.Get<IEnumerable<string>>(AllHolidayDatesCacheKey);

        //    if (allHolidays == null)
        //    {
        //        allHolidays = this.holidayService.AllDates();
        //        var cacheOptions = new MemoryCacheEntryOptions()
        //            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

        //        this.cache.Set(AllHolidayDatesCacheKey, allHolidays, cacheOptions);
        //    }
        public IEnumerable<HolidayServiceModel> All()
        {
            var allHolidays = this.cache.Get<IEnumerable<HolidayServiceModel>>(AllHolidaysCacheKey);

            if (allHolidays == null)
            {
                allHolidays = this.db.OfficialHolidays
                 .OrderBy(h => h.Date)
                 .ProjectTo<HolidayServiceModel>(this.mapper)
                 .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(AllHolidaysCacheKey, allHolidays, cacheOptions);

            }

            return allHolidays;
        }

        public IEnumerable<string> AllDates()
        {
            var allHolidayDates = this.cache.Get<IEnumerable<string>>(AllHolidayDatesCacheKey);

            if (allHolidayDates == null)
            {
                allHolidayDates = this.db.OfficialHolidays
                 .OrderBy(h => h.Date)
                 .Select(h => h.Date.ToLocalTime().Date.ToString("dd.MM.yyyy"))
                 .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(AllHolidayDatesCacheKey, allHolidayDates, cacheOptions);

            }

            return allHolidayDates;
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
