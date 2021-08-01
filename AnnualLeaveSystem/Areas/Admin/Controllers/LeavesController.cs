namespace AnnualLeaveSystem.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class LeavesController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
