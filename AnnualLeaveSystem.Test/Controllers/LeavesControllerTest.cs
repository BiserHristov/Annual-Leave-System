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
    using Microsoft.AspNetCore.Identity;

    public class LeavesControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthoriedUsersAndReturnView()
            => MyController<LeavesController>
            .Instance()
            .WithUser()
            .WithData(GetUser())
            .Calling(c => c.Add())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData("11.08.2021", "11.08.2021", 1)]
        public void PostAddShouldBeForAuthoriedUsersAndReturnView(string startDate, string endDate, int leaveType)
              => MyController<LeavesController>
            .Instance()
            .WithUser()
            .WithData(GetUser())
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
                    .To<StatisticsController>(c => c.History()));


        //.ActionAttributes(attributes => attributes
        //    .RestrictingForHttpMethod(HttpMethod.Post)
        //    .RestrictingForAuthorizedRequests())
        //.ValidModelState();



        private Employee GetUser()
          => new Employee()
          {
              Id = "ba795dfd-d305-4c52-a3e1-3fcd7ea12116",
              UserName = "Dimitrichka.Petkova@abv.bg",
              Email = "Dimitrichka.Petkova@abv.bg",
              PasswordHash = "AQAAAAEAACcQAAAAEN8/lF7jbkEiIKlSgZScynMTJV5QhMo2shtnLhlIRz0XhPJblUKVm4vJgVIYDQo2WA==",
          };



    }

}
