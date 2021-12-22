using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public DashboardController( OrderSystemContext context)
        {
            
            _context = context;
        }

        public IActionResult Index()
        {


            // month order data
            int monthShipmentOrderSum = (int)(from a in _context.ShipmentOrders
                                             where a.IsDeleted != true
                                             where a.FinishDate.Value.Month == DateTime.Now.Month
                                            select a.Total).Sum();
            int monthReturnShipmentOrderSum = (int)(from a in _context.ReturnShipmentOrders
                                                   where a.IsDeleted != true
                                                   where a.ReturnDate.Value.Month == DateTime.Now.Month
                                                   select a.Total).Sum();
            int monthOrderProfit = monthShipmentOrderSum - monthReturnShipmentOrderSum;
            ViewData["monthShipmentOrderSum"] = monthShipmentOrderSum;
            ViewData["monthReturnShipmentOrderSum"] = monthReturnShipmentOrderSum;
            ViewData["monthOrderProfit"] = monthOrderProfit;

            // year order data
            int yearShipmentOrderSum = (int)(from a in _context.ShipmentOrders
                                              where a.IsDeleted != true
                                              where a.FinishDate.Value.Year == DateTime.Now.Year
                                             select a.Total).Sum();
            int yearReturnShipmentOrderSum = (int)(from a in _context.ReturnShipmentOrders
                                                    where a.IsDeleted != true
                                                    where a.ReturnDate.Value.Year == DateTime.Now.Year
                                                   select a.Total).Sum();
            int yearOrderProfit = yearShipmentOrderSum - yearReturnShipmentOrderSum;
            ViewData["yearShipmentOrderSum"] = yearShipmentOrderSum;
            ViewData["yearReturnShipmentOrderSum"] = yearReturnShipmentOrderSum;
            ViewData["yearOrderProfit"] = yearOrderProfit;

            // cumulative data
            int allShipmentOrderCount = (int)(from a in _context.ShipmentOrders
                                              where a.IsDeleted != true
                                              select a.Id).Count();
            int allReturnShipmentOrderCount = (int)(from a in _context.ReturnShipmentOrders
                                      where a.IsDeleted != true
                                      select a.Id).Count();
            int allShipmentOrderCompletedCount = (int)(from a in _context.ShipmentOrders
                                                     where a.IsDeleted != true
                                                     where a.Status == Constant.OrderStatus.Completed
                                                     select a.Id).Count();

            int allShipmentOrderInProgressCount = (int)(from a in _context.ShipmentOrders
                                                       where a.IsDeleted != true
                                                       where a.Status == Constant.OrderStatus.InProgress
                                                       select a.Id).Count();
            int allProductCount = (int)(from a in _context.Products
                                                        where a.IsDeleted != true
                                                        select a.Id).Count();

            ViewData["allShipmentOrderCount"] = allShipmentOrderCount;
            ViewData["allReturnShipmentOrderCount"] = allReturnShipmentOrderCount;
            ViewData["allShipmentOrderCompletedCount"] = allShipmentOrderCompletedCount;
            ViewData["allShipmentOrderInProgressCount"] = allShipmentOrderInProgressCount;
            ViewData["allProductCount"] = allProductCount;

            // shipmentOrder delivery chart
            var shipmentOrderDelivery = (from a in _context.ShipmentOrders
                                                  where a.IsDeleted != true
                                                  where a.DeliveryDate.Value.Month == DateTime.Now.Month
                                                  group a by a.DeliveryDate.Value.Date into g
                                                  select new { DeliveryDate = g.Key, Count = g.Count() }).ToList();

            List<object> shipmentOrderDeliveryChartData = new List<object>();
            // Loop from the first day of the month until we hit the next month, moving forward a day at a time
            for (var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); date.Month == DateTime.Now.Month; date = date.AddDays(1))
            {
                var dayDelivery = shipmentOrderDelivery.Where(x => x.DeliveryDate == date).FirstOrDefault();
                if (dayDelivery != null)
                {
                    shipmentOrderDeliveryChartData.Add(new
                    {
                        DeliveryDate = ((DateTimeOffset)date).ToUnixTimeSeconds() * 1000,
                        Count = dayDelivery.Count
                    });
                }
                else
                {
                    shipmentOrderDeliveryChartData.Add(new
                    {
                        DeliveryDate = ((DateTimeOffset)date).ToUnixTimeSeconds() * 1000,
                        Count = 0
                    });
                }
            }
            ViewData["shipmentOrderDeliveryChartData"] = JsonConvert.SerializeObject(shipmentOrderDeliveryChartData);


            return View();
        }

    }
}
