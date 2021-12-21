using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderSystem.Models;
using OrderSystem.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Controllers
{
    public class DashboardController : Controller
    { 
        private readonly OrderSystemContext _context;
        private readonly ILogger<HomeController> _logger;

        public DashboardController(ILogger<HomeController> logger, OrderSystemContext context)
        {
            
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // get data test
            var model = _context.Users.Select(b => new User
            {
                Name = b.Name
            }).ToList();
            User m = model[0];

            return View();
        }
    }
}
