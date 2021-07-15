namespace AnnualLeaveSystem.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models;
    using AnnualLeaveSystem.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    public class HomeController : Controller
    {
        private readonly LeaveSystemDbContext db;

        public HomeController(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var homeModel = new IndexViewModel
            {
                EmployeesCount = this.db.Employees.Count(),
                ApprovedLeaveCount = this.db.Leaves.Count(l => l.Status == Status.Approved),
                InProgressLeaveCount = this.db.Leaves.Count(l => l.Status == Status.InProgress),
                AllLeavesTotalDays = this.db.Leaves.Sum(l => l.TotalDays),
            };

            return View(homeModel);

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

}
}
