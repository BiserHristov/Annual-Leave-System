namespace AnnualLeaveSystem.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using AnnualLeaveSystem.Controllers;
    using FluentAssertions;
    using static Data.Leaves;
    using AnnualLeaveSystem.Models.Home;
    using Microsoft.AspNetCore.Mvc;

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
