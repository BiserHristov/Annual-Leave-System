namespace AnnualLeaveSystem.Areas.Admin.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using AnnualLeaveSystem.Areas.Admin.Services.Holidays;
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Services.Holidays;

    using static AdminConstants.Holidays;

    public class HolidaysController : BaseAdminController
    {
        private readonly IHolidayService holidayService;
        private readonly IHolidayServiceAdmin holidayServiceAdmin;

        public HolidaysController(IHolidayService holidayService, IHolidayServiceAdmin holidayServiceAdmin)
        {
            this.holidayService = holidayService;
            this.holidayServiceAdmin = holidayServiceAdmin;
        }

        public IActionResult Add()
        {
            if (!this.User.IsAdmin())
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(HolidayServiceModel model)
        {
            if (!this.User.IsAdmin())
            {
                return Unauthorized();
            }

            ValidateHoliday(model, this.ModelState, holidayServiceAdmin);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.holidayServiceAdmin.Add(model);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Edit(int id)
        {
            if (!this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var holiday = this.holidayServiceAdmin.ById(id);
            if (holiday == null)
            {
                return BadRequest();
            }

            return View(holiday);
        }

        [HttpPost]
        public IActionResult Edit(HolidayServiceModel model)
        {
            if (!this.User.IsAdmin())
            {
                return Unauthorized();
            }

            ValidateHoliday(model, this.ModelState, holidayServiceAdmin, model.Id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = this.holidayServiceAdmin.Edit(model);
            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            var exist = this.holidayServiceAdmin.Exist(id);
            if (!exist)
            {
                return BadRequest();
            }

            var result = this.holidayServiceAdmin.Delete(id);
            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult All()
        {
            if (!this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var holidays = this.holidayService.All();
            return View(holidays);
        }

        private static void ValidateHoliday(
            HolidayServiceModel model,
            ModelStateDictionary modelState,
            IHolidayServiceAdmin holidayServiceAdmin,
            int id = 0)
        {
            if (!DateTime.TryParse(model.Date, out DateTime date))
            {
                modelState.AddModelError(nameof(model.Date), NotValidDateMessage);
            }
            else
            {
                var modelYear = date.Year;
                var nextYear = date.Year + 1;

                if (modelYear != nextYear)
                {
                    modelState.AddModelError(nameof(model.Date), NotNextYearMessage);
                }

                var exist = holidayServiceAdmin.Exist(DateTime.Parse(model.Date), id);
                if (exist)
                {
                    modelState.AddModelError(nameof(model.Date), AlreadyExistMessage);
                }
            }
        }
    }
}