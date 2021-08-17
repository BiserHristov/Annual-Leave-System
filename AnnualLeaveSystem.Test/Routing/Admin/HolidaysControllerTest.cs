namespace AnnualLeaveSystem.Test.Routing.Admin
{
    using AnnualLeaveSystem.Areas.Admin.Controllers;
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services.Holidays;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HolidaysControllerTest
    {
        [Fact]
        public void GetAddRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Holidays/Add")
            .To<HolidaysController>(c => c.Add());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Admin/Holidays/Add"))
            .To<HolidaysController>(c => c.Add(With.Any<HolidayServiceModel>()));

        [Fact]
        public void GetEditRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Holidays/Edit/5")
            .To<HolidaysController>(c => c.Edit(5));

        [Fact]
        public void PostEditRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Admin/Holidays/Edit/5"))
            .To<HolidaysController>(c => c.Edit(With.Any<HolidayServiceModel>()));

        [Fact]
        public void GetDeleteRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Holidays/Delete/5")
            .To<HolidaysController>(c => c.Delete(5));

        [Fact]
        public void GetAllRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Holidays/All")
            .To<HolidaysController>(c => c.All());
    }
}
