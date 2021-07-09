﻿namespace AnnualLeaveSystem.Controllers
{
    using System.Diagnostics;
    using AnnualLeaveSystem.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    public class HomeController : Controller
    {

        public IActionResult Index()=>View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()=>View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
