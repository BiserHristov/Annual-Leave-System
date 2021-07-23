namespace AnnualLeaveSystem.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models;
    using AnnualLeaveSystem.Models.Home;
    using AnnualLeaveSystem.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;

        public HomeController( IStatisticsService statistics)
        {
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            var homeModel = new IndexViewModel
            {
                EmployeesCount = this.statistics.Get().EmployeesCount,
                ApprovedLeaveCount = this.statistics.Get().ApprovedLeaveCount,
                InProgressLeaveCount = this.statistics.Get().InProgressLeaveCount,
                AllLeavesTotalDays = this.statistics.Get().AllLeavesTotalDays
            };

            return View(homeModel);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }
}
