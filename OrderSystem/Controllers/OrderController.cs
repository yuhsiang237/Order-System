using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult ShipmentOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShipmentOrderCreate(ShipmentOrderCreateViewModel m)
        {
            ShipmentOrderCreateValidator validator = new ShipmentOrderCreateValidator();
            ValidationResult result = validator.Validate(m);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null,null,0,result.Errors));
            }
            // vaildate data
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
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
                    }
                    // set total price
                    o.Total = calcTotal;
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
           
            return View();
        }
        public IActionResult ReturnOrder()
        {
            return View();
        }
    }
}
