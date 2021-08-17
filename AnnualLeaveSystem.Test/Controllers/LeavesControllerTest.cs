namespace AnnualLeaveSystem.Test.Controllers
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Test.Data;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class LeavesControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthoriedUsersAndReturnView()
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

        //[Theory]
        //[InlineData("25.08.2021", "25.08.2021", 1)]
        //public void PostAddShouldBeForAuthoriedUsersAndReturnView(string startDate, string endDate, int leaveType)
        //      => MyController<LeavesController>
        //               .Instance()
        //              .WithUser(x => x.WithIdentifier(EmployeeTestData.UserId))
        //               .WithData(EmployeeTestData.Create())
        //              .Calling(c => c.Add(new LeaveFormModel
        //              {
        //                  StartDate = DateTime.Parse(startDate),
        //                  EndDate = DateTime.Parse(endDate),
        //                  LeaveTypeId = leaveType,
        //                  SubstituteEmployeeId = EmployeeTestData.SubstituteId,
        //              }))
        //              .ShouldHave()
        //              .ValidModelState()
        //            .AndAlso()
        //            .ShouldHave()
        //        .Data(data => data
        //    .WithSet<Leave>(leaves => leaves
        //    .Any(l =>
        //        l.StartDate.ToLocalTime().Date.ToString("dd.MM.yyyy") == startDate &&
        //        l.EndDate.ToLocalTime().Date.ToString("dd.MM.yyyy") == endDate &&
        //        l.LeaveTypeId == leaveType &&
        //        l.SubstituteEmployeeId == EmployeeTestData.SubstituteId
        //        )))
        //.AndAlso()
        //    .ShouldReturn()
        //    .Redirect(redirect => redirect
        //        .To<StatisticsController>(c => c.History()))
        //.ActionAttributes(attributes => attributes
        //    .RestrictingForHttpMethod(HttpMethod.Post)
        //    .RestrictingForAuthorizedRequests())
        //.ValidModelState();

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