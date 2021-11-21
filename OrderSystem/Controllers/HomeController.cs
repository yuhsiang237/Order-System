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
    public class HomeController : Controller
    { 
        private readonly OrderSystemContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, OrderSystemContext context)
        {
            
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // store data test
            Order o = new Order();
            o.Number = "12153";
            o.Status = 1;
            o.Total = 100;
            o.SignName = "123";
            o.UpdateDate = DateTime.Now;
            o.DeliveryDate = DateTime.Now;
            o.FinishDate = DateTime.Now;

            _context.Orders.Add(o);
            _context.SaveChanges();

            // get data test
            var model = _context.Users.Select(b => new User
            {
                Name = b.Name
            }).ToList();
            User m = model[0];

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       

    }
}
