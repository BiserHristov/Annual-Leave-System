namespace AnnualLeaveSystem.Controllers.Api
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using AnnualLeaveSystem.Services.Holidays;

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
            => this.holidayService.All();
    }
}
