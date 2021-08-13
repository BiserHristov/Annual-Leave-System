namespace AnnualLeaveSystem.Areas.Admin.Services.Holidays
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Services.Holidays;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HolidayServiceAdmin : IHolidayServiceAdmin
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;
        public HolidayServiceAdmin(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Add(HolidayServiceModel model)
        {
            var holiday = new OfficialHoliday
            {
                Date = DateTime.Parse(model.Date).ToUniversalTime(),
                Name = model.Name
            };

            this.db.OfficialHolidays.Add(holiday);
            this.db.SaveChanges();
        }

        public HolidayServiceModel ById(int holidayId)
        {
            return this.db.OfficialHolidays
                .Where(h => h.Id == holidayId)
                .ProjectTo<HolidayServiceModel>(this.mapper)
                .FirstOrDefault();
        }

        public bool Delete(int Id)
        {
            var holiday = this.db.OfficialHolidays.Find(Id);

            if (holiday == null)
            {
                return false;
            }

            this.db.OfficialHolidays.Remove(holiday);
            this.db.SaveChanges();

            return true;
        }

        public bool Edit(HolidayServiceModel model)
        {
            var holiday = this.db.OfficialHolidays.Find(model.Id);

            if (holiday == null)
            {
                return false;
            }

            holiday.Date = DateTime.Parse(model.Date).ToUniversalTime();
            holiday.Name = model.Name;

            this.db.SaveChanges();
            return true;
        }

        public bool Exist(DateTime date, int id)
        {
            var holiday = this.db.OfficialHolidays
                .Where(h => h.Date == date)
                .AsQueryable();

            if (id == 0)
            {
                return holiday.Any();
            }

            return holiday.Any(h => h.Id != id);
        }

        public bool Exist(int Id)
        {
            return this.db.OfficialHolidays.Any(h => h.Id == Id);
        }
        //public bool Exist(DateTime date)
        //{
        //    return this.db.OfficialHolidays.Any(h => h.Date == date);
        //}
    }
}
