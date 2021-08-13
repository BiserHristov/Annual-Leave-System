namespace AnnualLeaveSystem.Test.Pipeline
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Home;
    using MyTested.AspNetCore.Mvc;
    using FluentAssertions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Leaves;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
            .Pipeline()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index())
            .Which(c => c.WithData(TenApprovedLeaves()))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<IndexViewModel>()
                .Passing(m => m.ApprovedLeaveCount.Should().Be(10)));


        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap("/Home/Error")
            .To<HomeController>(c => c.Error())
            .Which()
            .ShouldReturn()
            .View();
    }
}
