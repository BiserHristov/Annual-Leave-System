namespace AnnualLeaveSystem.Areas.Admin.Services.Holidays
{
    using System;
    using AnnualLeaveSystem.Services.Holidays;

    public interface IHolidayServiceAdmin
    {
        public HolidayServiceModel ById(int holidayId);

        public void Add(HolidayServiceModel model);

        public bool Edit(HolidayServiceModel model);

        public bool Delete(int id);

        public bool Exist(DateTime date, int id = 0);

        public bool Exist(int id);
    }
}