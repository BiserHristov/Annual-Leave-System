namespace AnnualLeaveSystem.Services.Holidays
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.Extensions.Caching.Memory;
    using AnnualLeaveSystem.Data;

    using static WebConstants;
    using static WebConstants.Cache;

    public class HolidayService : IHolidayService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;
        private readonly IMemoryCache cache;

        public HolidayService(
            LeaveSystemDbContext db, 
            IMapper mapper, 
            IMemoryCache cache)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
            this.cache = cache;
        }

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
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

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
                 .Select(h => h.Date.ToLocalTime().Date.ToString(DateFormat))
                 .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                this.cache.Set(AllHolidayDatesCacheKey, allHolidayDates, cacheOptions);
            }

            return allHolidayDates;
        }

        //public (bool, string) IsHoliday(DateTime date)
        //{
        //    var holiday = this.db.OfficialHolidays
        //        .Where(h => h.Date == date.Date)
        //        .FirstOrDefault();

        //    if (holiday == null)
        //    {
        //        return (false, string.Empty);
        //    }

        //    return (true, holiday.Name);
        //}
    }
}
