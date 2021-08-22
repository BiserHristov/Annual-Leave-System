namespace AnnualLeaveSystem.Test.Controllers
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Test.Data;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using Xunit;

    public class LeavesControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyController<LeavesController>
            .Instance()
            .WithUser()
            .WithData(EmployeeTestData.Create())
            .Calling(c => c.Add())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(v => v.WithModelOfType<LeaveFormModel>());

        [Theory]
        [InlineData("25.08.2021", "25.08.2021", 1)]
        public void PostAddShouldBeForAuthoriedUsersAndReturnView(string startDate, string endDate, int leaveType)
              => MyController<LeavesController>
            .Instance()
            .WithUser(x => x.WithIdentifier(EmployeeTestData.UserId))
            .WithData(EmployeeTestData.Create())
            .Calling(c => c.Add(new LeaveFormModel
            {
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
                TotalDays = 1,
                LeaveTypeId = leaveType,
                RequestEmployeeId = EmployeeTestData.UserId,
                SubstituteEmployeeId = EmployeeTestData.SubstituteId,
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
                    l.SubstituteEmployeeId == EmployeeTestData.SubstituteId
                    )))
            .AndAlso()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<LeavesController>(c => c.All(With.Any<AllLeavesQueryModel>())));

        [Fact]
        public void GetAllShouldReturnView()
             => MyController<LeavesController>
             .Instance()
             .WithUser()
             .WithData(TestData.GetUser())
             .Calling(c => c.All(new AllLeavesQueryModel
             {
                 Status = null,
                 FirstName = null,
                 LastName = null,
                 Sorting = LeaveSorting.StartDate,
                 CurrentPage = 1,
             }))
             .ShouldReturn()
             .View(v => v.WithModelOfType<AllLeavesQueryModel>());
    }
}