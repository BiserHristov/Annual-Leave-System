namespace AnnualLeaveSystem.Controllers
{
    using AnnualLeaveSystem.Infrastructure;
    using AnnualLeaveSystem.Services.Leaves;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StatisticController : Controller
    {
        private readonly ILeaveService leaveService;

        public StatisticController(ILeaveService leaveService)
        {
            this.leaveService = leaveService;
        }

        [Authorize]
        public IActionResult History()
        {
            var leaves = this.leaveService.ByEmployee(this.User.GetId());

            return View(leaves);
        }
    }
}
