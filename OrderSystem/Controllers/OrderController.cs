using Microsoft.AspNetCore.Mvc;
using OrderSystem.Models;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderSystemContext _context;

        public OrderController(OrderSystemContext context)
        {
            _context = context;
        }

        public IActionResult ShipmentOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShipmentOrderCreate(Order model)
        {
            // order test
            Order o = new Order();
            o.Number = "S_1190831090";
            o.Type = 0;
            o.DeliveryDate = model.DeliveryDate;
            o.FinishDate = model.FinishDate;
            o.Total = model.Total;
            o.Remarks = model.Remarks;
            o.Address = model.Address;

            return Ok(ResponseModel.Success(""));

        }

        [HttpGet]
        public IActionResult ShipmentOrderCreate()
        {
           
            return View();
        }
        public IActionResult ReturnOrder()
        {
            return View();
        }
    }
}
