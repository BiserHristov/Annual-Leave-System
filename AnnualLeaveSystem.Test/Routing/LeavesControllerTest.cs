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
        public void GetAllRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Leaves/All")
            .To<LeavesController>(c => c.All(With.Any<AllLeavesQueryModel>()));

        [Fact]
        public void GetDetailsRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Leaves/Details?leaveId=4")
            .To<LeavesController>(c => c.Details(4));

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
                 .WithLocation("/Leaves/Edit?leaveId=5"))
             .To<LeavesController>(c => c.Edit(5, With.Any<LeaveFormModel>()));

        [Fact]
        public void GetCancelRouteShouldBeMapped()
             => MyRouting
             .Configuration()
             .ShouldMap(request => request
                 .WithMethod(HttpMethod.Post)
                 .WithLocation("/Leaves/Cancel?leaveId=5"))
             .To<LeavesController>(c => c.Cancel(5));

        [Fact]
        public void GetRejectRouteShouldBeMapped()
             => MyRouting
             .Configuration()
             .ShouldMap(request => request
                 .WithMethod(HttpMethod.Post)
                 .WithLocation("/Leaves/Reject?leaveId=5"))
             .To<LeavesController>(c => c.Reject(5));

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
               .WithLocation("/Leaves/ForApproval?leaveId=1"))
            .To<LeavesController>(c => c.ForApproval(1));


    }
}
