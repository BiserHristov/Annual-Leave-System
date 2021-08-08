namespace AnnualLeaveSystem.Areas.Admin.Services.Holidays
{
    using AnnualLeaveSystem.Services.Holidays;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IHolidayServiceAdmin
    {
        public HolidayServiceModel ById(int holidayId);
        public void Add(HolidayServiceModel model);
        public bool Edit(HolidayServiceModel model);
        public bool Delete(int Id);
        public bool Exist(DateTime date, int id=0);
        public bool Exist(int Id);

    }
}
