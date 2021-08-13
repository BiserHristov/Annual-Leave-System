namespace AnnualLeaveSystem.Controllers.Api
{
    using AnnualLeaveSystem.Services.Holidays;
    using AnnualLeaveSystem.Data;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("Leaves/api/holidays")]
    public class HolidaysApiController : ControllerBase
    {
        private readonly IHolidayService holidayService;

        public HolidaysApiController(IHolidayService holidayService)
        {
            this.holidayService = holidayService;
        }

        [HttpGet]
        public IEnumerable<HolidayServiceModel> GetHolidays()
        {
            var result = this.holidayService.All();
             return result;

        }
    }
}
