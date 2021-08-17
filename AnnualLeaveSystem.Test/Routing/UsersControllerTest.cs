namespace AnnualLeaveSystem.Test.Routing
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Models.Users;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class UsersControllerTest
    {
        [Fact]
        public void GetRegisterRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Users/Register")
            .To<UsersController>(c => c.Register());

        [Fact]
        public void PostRegisterRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Users/Register"))
            .To<UsersController>(c => c.Register(With.Any<EmployeeFormModel>()));

        [Fact]
        public void GetLoginRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Users/Login")
            .To<UsersController>(c => c.Login());

        [Fact]
        public void PostLoginRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Users/Login"))
            .To<UsersController>(c => c.Login(With.Any<LoginFormModel>()));

        [Fact]
        public void GetLogoutRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Users/Logout")
            .To<UsersController>(c => c.Logout());


    }
}
