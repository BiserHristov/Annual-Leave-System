namespace AnnualLeaveSystem.Services.Holidays
{
    using System;
    using System.Collections.Generic;

    public interface IHolidayService
    {
        public (bool isHoliday, string name) IsHoliday(DateTime date);

        public IEnumerable<string> AllDates();

        public IEnumerable<HolidayServiceModel> All();
    }
}