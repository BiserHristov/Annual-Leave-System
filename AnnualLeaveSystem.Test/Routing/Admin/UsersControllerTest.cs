namespace AnnualLeaveSystem.Test.Routing.Admin
{
    using AnnualLeaveSystem.Areas.Admin.Controllers;
    using AnnualLeaveSystem.Models.Users;
    using AnnualLeaveSystem.Services.Holidays;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.TestData;
    public class UsersControllerTest
    {
        [Fact]
        public void GetAddRouteShouldBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/Admin/Users/Add")
           .To<UsersController>(c => c.Add());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Admin/Users/Add"))
            .To<UsersController>(c => c.Add(With.Any<EmployeeFormModel>()));

        [Fact]
        public void GetEditRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Users/Edit/" + UserId)
            .To<UsersController>(c => c.Edit(UserId));

        [Fact]
        public void PostEditRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Admin/Users/Edit/" + UserId))
            .To<UsersController>(c => c.Edit(With.Any<EmployeeFormModel>()));

        [Fact]
        public void GetDeleteRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Users/Delete/" + UserId)
            .To<UsersController>(c => c.Delete(UserId));

        [Fact]
        public void GetAllRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Users/All")
            .To<UsersController>(c => c.All());
    }
}
