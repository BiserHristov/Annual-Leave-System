namespace AnnualLeaveSystem.Test.Controllers
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Models.Home;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    using static Data.LeaveTestData;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldRetutnCorrectViewWithModel()
            => MyController<HomeController>
            .Instance(instance => instance
                .WithData(TenApprovedLeaves()))
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<IndexViewModel>()
                .Passing(model => model.ApprovedLeaveCount.Should().Be(10)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(c => c.Error())
            .ShouldReturn()
            .View();
    }
}
