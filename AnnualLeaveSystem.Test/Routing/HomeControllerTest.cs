namespace AnnualLeaveSystem.Test.Routing
{
    using AnnualLeaveSystem.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void GetIndexRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index());

        [Fact]
        public void GetErrorRouteShouldBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/Home/Error")
           .To<HomeController>(c => c.Error());
    }
}
