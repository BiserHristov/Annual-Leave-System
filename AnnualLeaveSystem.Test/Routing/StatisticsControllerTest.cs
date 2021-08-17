namespace AnnualLeaveSystem.Test.Routing
{
    using AnnualLeaveSystem.Controllers;
    using AnnualLeaveSystem.Models.Leaves;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class StatisticsControllerTest
    {
        [Fact]
        public void GetHistoryRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Statistics/History")
            .To<StatisticsController>(c => c.History());
    }
}
