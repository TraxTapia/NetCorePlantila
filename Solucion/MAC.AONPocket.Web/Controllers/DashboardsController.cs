using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MAC.AONPocket.Web.Controllers
{
    public class DashboardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard_1()
        {
            return View();
        }
    }
}