namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Models.Home;
    using AnnualLeaveSystem.Services.Statistics;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    public class HomeController : Controller
    {
        private readonly ICommonInfoService statistics;
        private readonly IMapper mapper;

        public HomeController(ICommonInfoService statistics, IMapper mapper)
        {
            this.statistics = statistics;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var allStatistics = this.statistics.Get();

            var homeModel = this.mapper.Map<IndexViewModel>(allStatistics);

            return View(homeModel);

        }

        public IActionResult Error() => View();

    }
}
