using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderSystem.Commons;
using OrderSystem.Models;
using OrderSystem.Models.Validator;
using OrderSystem.Tools;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OrderSystem.Models.Constant;

namespace OrderSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderSystemContext _context;

        public OrderController(OrderSystemContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ShipmentOrder(
     string sortOrder,
     string currentFilterNumber,
     string searchStringNumber,
     string currentFilterName,
     string searchStringName,
     int? goToPageNumber,
     int pageSize,
     int? pageNumber)
        {
            // 1.search logic
            var query = from a in _context.Orders
                        where a.Type == OrderType.Shipment
                        where a.IsDeleted != true
                        select new ShipmentOrderViewModel { 
                             Id = a.Id,
                             Number = a.Number,
                             Total = a.Total,
                             SignName = a.SignName,
                             Status = a.Status,
                             DeliveryDate = a.DeliveryDate,
                             FinishDate = a.FinishDate
                        };

            // 2.condition filter
            if (searchStringNumber != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchStringName = currentFilterName;
            }

            ViewData["CurrentFilterNumber"] = searchStringNumber;

            if (!String.IsNullOrEmpty(searchStringNumber))
            {
                query = query.Where(s => s.Number.Contains(searchStringNumber));
            }
            // 3.sort data
            ViewData["CurrentSort"] = sortOrder;

            switch (sortOrder)
            {
                case "0":
                    query = query.OrderByDescending(s => s.Id);
                    break;
                case "1":
                    query = query.OrderByDescending(s => s.Number);
                    break;
                case "2":
                    query = query.OrderBy(s => s.Number);
                    break;
                default:
                    query = query.OrderByDescending(s => s.Id);
                    break;
            }

            // 4.go page
            if (goToPageNumber != null)
            {
                pageNumber = goToPageNumber;
            }

            // 5.per page count
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            ViewData["pageSize"] = pageSize;

            // 6.result
            return View(await PaginatedList<ShipmentOrderViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        
        [HttpPost]
        public IActionResult ShipmentOrderUpdate(ShipmentOrderUpdateViewModel m)
        {
            ShipmentOrderUpdateValidator validator = new ShipmentOrderUpdateValidator(_context);
            ValidationResult result = validator.Validate(m);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
            }
            var Order = _context.Orders.FirstOrDefault(x=>x.Id == m.Order.Id);
            Order.FinishDate = m.Order.FinishDate;
            Order.DeliveryDate = m.Order.DeliveryDate;
            Order.Address = m.Order.Address;
            Order.SignName = m.Order.SignName;
            Order.Remarks = m.Order.Remarks;
            if(Order.FinishDate != null)
            {
                Order.Status = Constant.OrderStatus.Completed;
            }
            else if((Order.FinishDate == null))
            {
                Order.Status = Constant.OrderStatus.InProgress;
            }
            _context.Update(Order);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }
        [HttpPost]
        public IActionResult ShipmentOrderCreate(ShipmentOrderCreateViewModel m)
        {
            // vaildate data
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    ShipmentOrderCreateValidator validator = new ShipmentOrderCreateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // add order & order details
                    Order o = new Order();
                    o.Number = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Shipment);
                    o.Type = OrderType.Shipment;
                    o.DeliveryDate = m.Order.DeliveryDate;
                    o.FinishDate = m.Order.FinishDate;
                    o.Remarks = m.Order.Remarks;
                    o.Address = m.Order.Address;
                    o.SignName = m.Order.SignName;
                    _context.Orders.Add(o);
                    _context.SaveChanges();

                    decimal calcTotal = 0;
                    foreach (var item in m.OrderDetails)
                    {
                        var product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                        // get current product status 
                        item.OrderId = o.Id;
                        item.ProductName = product.Name;
                        item.ProductNumber = product.Number;
                        item.ProductPrice = product.Price;
                        calcTotal += item.ProductPrice.Value * item.ProductUnit.Value;
                        _context.OrderDetails.Add(item);
                        _context.SaveChanges();
                        // ProductInventory add change record
                        ProductInventory pi = new ProductInventory();
                        pi.ProductId = product.Id;
                        pi.Unit = -1*item.ProductUnit.Value;
                        pi.Description = ProductInventoryChangeCode.ShipmentOrder +":"+ o.Number;
                        pi.CreatedAt = DateTime.Now;
                        // product update CurrentUnit
                        product.CurrentUnit = product.CurrentUnit - item.ProductUnit.Value;
                        _context.Update(product);
                        _context.ProductInventories.Add(pi);
                        _context.SaveChanges();
                    }

                    // set total price
                    o.Total = calcTotal;
                    if (o.FinishDate != null)
                    {
                        o.Status = OrderStatus.Completed;
                    }
                    else
                    {
                        o.Status = OrderStatus.InProgress;
                    }
                    _context.Update(o);
                    _context.SaveChanges();

                    tr.Commit();
                    return Ok(ResponseModel.Success(""));
                }
                catch (Exception ex)
                {
                    return Ok(ResponseModel.Fail("建立失敗", null, 0, ""));
                }
            }

        }

        [HttpGet]
        public IActionResult ShipmentOrderCreate()
        {
            ViewData["ProductData"] = JsonConvert.SerializeObject((from a in _context.Products
                                      where a.IsDeleted != true
                                      select a).ToList());
            return View();
        }
        [HttpGet]
        public IActionResult ShipmentOrderEdit(int OrderId)
        {
            ViewData["Order"] = JsonConvert.SerializeObject(_context.Orders.FirstOrDefault(x => x.Id == OrderId));
            ViewData["OrderDetails"] = JsonConvert.SerializeObject((from a in _context.OrderDetails
                                   where a.OrderId == OrderId
                                   select a).ToList());
            return View();
        }
        public IActionResult ReturnOrder()
        {
            return View();
        }
    }
}
