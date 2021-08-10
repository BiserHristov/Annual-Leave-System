namespace AnnualLeaveSystem.Test.Controllers
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

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
            .Pipeline()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index())
            .Which(c => c.WithData(GetApprovedLeaves()))
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<IndexViewModel>()
            .Passing(m => m.ApprovedLeaveCount.Should().Be(10)));



        private static IEnumerable<Leave> GetApprovedLeaves()
        {
            return Enumerable.Range(0, 10).Select(x => new Leave()
            {
                LeaveStatus=Status.Approved
            });
        }
    }


}
