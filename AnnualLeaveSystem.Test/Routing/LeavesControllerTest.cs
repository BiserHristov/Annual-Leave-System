namespace AnnualLeaveSystem.Test.Routing
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Models.Leaves;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class LeavesControllerTest
    {
        [Fact]
        public void GetAddRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Leaves/Add")
            .To<LeavesController>(c => c.Add());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithPath("/Leaves/Add"))
            .To<LeavesController>(c => c.Add(With.Any<LeaveFormModel>()));

        [Fact]
        public void GetEditRouteShouldBeMapped()
        => MyRouting
        .Configuration()
        .ShouldMap("/Leaves/Edit?leaveId=5")
        .To<LeavesController>(c => c.Edit(5));

        [Fact]
        public void PostEditRouteShouldBeMapped()
                => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithPath(@"/Leaves/Edit?leaveId=5"))
                .To<LeavesController>(c => c.Edit(5, With.Any<LeaveFormModel>()));

        [Fact]
        public void GetForApprovalRouteShouldBeMapped()
             => MyRouting
             .Configuration()
             .ShouldMap("/Leaves/ForApproval")
             .To<LeavesController>(c => c.ForApproval());

        [Fact]
        public void PostForApprovalRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
               .WithMethod(HttpMethod.Post)
               .WithPath("/Leaves/ForApproval?leaveId=1"))
            .To<LeavesController>(c => c.ForApproval(1));


    }
}
