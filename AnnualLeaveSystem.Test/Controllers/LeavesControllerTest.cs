namespace AnnualLeaveSystem.Test.Controllers
{

    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using AnnualLeaveSystem.Controllers;
    using FluentAssertions;
    using static Data.Leaves;
    using AnnualLeaveSystem.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using AnnualLeaveSystem.Data.Models;
    using System.Linq;

    public class LeavesControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthoriedUsersAndReturnView()
            => MyController<LeavesController>
            .Instance()
            .Calling(c => c.Add())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(With.Any<LeaveFormModel>());

        [Theory]
        [InlineData("11.08.2021", "11.08.2021", 1)]
        public void PostAddShouldBeForAuthoriedUsersAndReturnView(string startDate, string endDate, int leaveType)
              => MyController<LeavesController>
                      .Instance(instance => instance
                          .WithUser(user=> user
                            .WithUsername("Dimitrichka.Petkova@abv.bg")
                          .WithIdentifier("0868d674-9200-4d63-be56-5b60498de4af")))
                      .Calling(c => c.Add(new LeaveFormModel
                      {
                          StartDate = DateTime.Parse(startDate),
                          EndDate = DateTime.Parse(endDate),
                          LeaveTypeId = leaveType,
                          SubstituteEmployeeId = substituteEmployeeId,
                      }))
                      .ShouldHave()
                      .ValidModelState()
                        .AndAlso()
                        .ShouldHave()
                    .Data(data => data
                .WithSet<Leave>(leaves => leaves
                .Any(l =>
                    l.StartDate.ToLocalTime().Date.ToString("dd.MM.yyyy") == startDate &&
                    l.EndDate.ToLocalTime().Date.ToString("dd.MM.yyyy") == endDate &&
                    l.LeaveTypeId == leaveType &&
                    l.SubstituteEmployeeId == substituteEmployeeId
                    )))
            .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<StatisticsController>(c => c.History()))
            .AndAlso()
            .ShouldPassForThe<LeavesController>(c=>
            {
                var smtg = c.ModelState.IsValid;
            });

        //.ActionAttributes(attributes => attributes
        //    .RestrictingForHttpMethod(HttpMethod.Post)
        //    .RestrictingForAuthorizedRequests())
        //.ValidModelState();





    }

}
