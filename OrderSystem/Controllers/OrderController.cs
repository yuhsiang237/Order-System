using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderSystem.Authorization;
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
        [PermissionFilter(Permissions.Order_Shipment_View)]
        [HttpGet]

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
            var query = from a in _context.ShipmentOrders
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
        [PermissionFilter(Permissions.Order_ReturnShipment_View)]
        [HttpGet]

        public async Task<IActionResult> ReturnShipmentOrder(
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
            var query = from a in _context.ReturnShipmentOrders
                        join b in _context.ShipmentOrders
                        on a.ShipmentOrderId equals b.Id
                        where a.IsDeleted != true
                        select new ReturnShipmentOrderViewModel
                        {
                            Id = a.Id,
                            Number = a.Number,
                            ShipmentOrderNumber = b.Number,
                            Total = a.Total,
                            ReturnDate = a.ReturnDate
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
            return View(await PaginatedList<ReturnShipmentOrderViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        [PermissionFilter(Permissions.Order_Shipment_Modify)]

        public IActionResult ShipmentOrderUpdate(ShipmentOrderUpdateViewModel m)
        {
            ShipmentOrderUpdateValidator validator = new ShipmentOrderUpdateValidator(_context);
            ValidationResult result = validator.Validate(m);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
            }
            var Order = _context.ShipmentOrders.FirstOrDefault(x=>x.Id == m.ShipmentOrder.Id);
            Order.FinishDate = m.ShipmentOrder.FinishDate;
            Order.DeliveryDate = m.ShipmentOrder.DeliveryDate;
            Order.Address = m.ShipmentOrder.Address;
            Order.SignName = m.ShipmentOrder.SignName;
            Order.Remarks = m.ShipmentOrder.Remarks;
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
        [PermissionFilter(Permissions.Order_ReturnShipment_Create)]

        public IActionResult ReturnShipmentOrderCreate(ReturnShipmentOrderCreateViewModel m)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    ReturnShipmentOrderCreateValidator validator = new ReturnShipmentOrderCreateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // add return order & return order details
                    ReturnShipmentOrder rso = new ReturnShipmentOrder();
                    rso.Number = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Return);
                    rso.ShipmentOrderId = m.ReturnShipmentOrder.ShipmentOrderId;
                    rso.ReturnDate = m.ReturnShipmentOrder.ReturnDate;
                    rso.Remarks = m.ReturnShipmentOrder.Remarks;
                    _context.ReturnShipmentOrders.Add(rso);
                    _context.SaveChanges();
                    decimal _total = 0;
                    foreach (var item in m.ReturnShipmentOrderDetails)
                    {
                        ReturnShipmentOrderDetail rsod = new ReturnShipmentOrderDetail();
                        rsod.ReturnShipmentOrderId = rso.Id;
                        rsod.ShipmentOrderDetailId = item.ShipmentOrderDetailId;
                        rsod.Remarks = item.Remarks;
                        rsod.Unit = item.Unit.HasValue ? item.Unit:0;
                        // calc total
                        var shipmentOrderDetail = _context.ShipmentOrderDetails.FirstOrDefault(x => x.Id == item.ShipmentOrderDetailId);
                        _total += shipmentOrderDetail.ProductPrice.Value * rsod.Unit.Value;
                        _context.ReturnShipmentOrderDetails.Add(rsod);
                        // ProductInventory add change record
                        if(rsod.Unit > 0)
                        {
                            ProductInventory pi = new ProductInventory();
                            pi.ProductId = shipmentOrderDetail.ProductId;
                            pi.Unit = rsod.Unit;
                            pi.Description = ProductInventoryChangeCode.ReturnShipmentOrder + ":" + rso.Number;
                            pi.CreatedAt = DateTime.Now;
                            // product update CurrentUnit
                            var product = _context.Products.FirstOrDefault(x => x.Id == shipmentOrderDetail.ProductId);
                            product.CurrentUnit = product.CurrentUnit + rsod.Unit;
                            _context.Update(product);
                            _context.ProductInventories.Add(pi);
                            _context.SaveChanges();
                        }
                    }
                    rso.Total = _total;
                    _context.Update(rso);
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
        [HttpPost]
        [PermissionFilter(Permissions.Order_ReturnShipment_Modify)]

        public IActionResult ReturnShipmentOrderUpdate(ReturnShipmentOrderCreateViewModel m)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    ReturnShipmentOrderUpdateValidator validator = new ReturnShipmentOrderUpdateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // add return order & return order details
                    var rso = _context.ReturnShipmentOrders.FirstOrDefault(x => x.Id == m.ReturnShipmentOrder.Id);
                    rso.ReturnDate = m.ReturnShipmentOrder.ReturnDate;
                    rso.Remarks = m.ReturnShipmentOrder.Remarks;
                    _context.Update(rso);
                    _context.SaveChanges();
                    var rsod = (from a in _context.ReturnShipmentOrderDetails
                               where a.ReturnShipmentOrderId == rso.Id
                               select a).ToList();

                    decimal _total = 0;
                    foreach (var item in rsod)
                    {
                        // new detail val
                        var newRsodVal = m.ReturnShipmentOrderDetails.FirstOrDefault(x => x.Id == item.Id); 

                        item.Remarks = newRsodVal.Remarks;
                        decimal? _OldUnit = item.Unit;
                        item.Unit = newRsodVal.Unit.HasValue ? newRsodVal.Unit : 0;

                        // calc total
                        var shipmentOrderDetail = _context.ShipmentOrderDetails.FirstOrDefault(x => x.Id == item.ShipmentOrderDetailId);
                        _total += shipmentOrderDetail.ProductPrice.Value * item.Unit.Value;
                        _context.Update(item);

                        // ProductInventory add change record
                        ProductInventory pi = new ProductInventory();
                        pi.ProductId = shipmentOrderDetail.ProductId;
                        pi.Unit = _OldUnit - newRsodVal.Unit;
                        if (pi.Unit != 0)
                        {
                            pi.Description = ProductInventoryChangeCode.ReturnShipmentOrderUpdate + ":" + rso.Number + "。" +"退貨數量調整"+ _OldUnit+ "調整為"+ (newRsodVal.Unit.Value).ToString("#0.0000");


                            pi.CreatedAt = DateTime.Now;
                            // product update CurrentUnit
                            var product = _context.Products.FirstOrDefault(x => x.Id == shipmentOrderDetail.ProductId);
                            product.CurrentUnit = product.CurrentUnit + pi.Unit;
                            _context.Update(product);
                            _context.ProductInventories.Add(pi);
                            _context.SaveChanges();
                        }
                    }
                    rso.Total = _total;
                    _context.Update(rso);
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
        [HttpPost]
        [PermissionFilter(Permissions.Order_Shipment_Create)]

        public IActionResult ShipmentOrderCreate(ShipmentOrderCreateViewModel m)
        {
          
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    ShipmentOrderCreateValidator validator = new ShipmentOrderCreateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // add order & order details
                    ShipmentOrder o = new ShipmentOrder();
                    o.Number = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Shipment);
                    o.Type = OrderType.Shipment;
                    o.DeliveryDate = m.ShipmentOrder.DeliveryDate;
                    o.FinishDate = m.ShipmentOrder.FinishDate;
                    o.Remarks = m.ShipmentOrder.Remarks;
                    o.Address = m.ShipmentOrder.Address;
                    o.SignName = m.ShipmentOrder.SignName;
                    _context.ShipmentOrders.Add(o);
                    _context.SaveChanges();

                    decimal calcTotal = 0;
                    foreach (var item in m.ShipmentOrderDetails)
                    {
                        var product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                        // get current product status 
                        item.OrderId = o.Id;
                        item.ProductName = product.Name;
                        item.ProductNumber = product.Number;
                        item.ProductPrice = product.Price;
                        calcTotal += item.ProductPrice.Value * item.ProductUnit.Value;
                        _context.ShipmentOrderDetails.Add(item);
                        _context.SaveChanges();
                        // ProductInventory add change record
                        ProductInventory pi = new ProductInventory();
                        pi.ProductId = product.Id;
                        pi.Unit = -1*item.ProductUnit.Value;
                        pi.Description = ProductInventoryChangeCode.ShipmentOrder +":"+ o.Number;
                        pi.CreatedAt = DateTime.Now;
                        // product update CurrentUnit
                        product.CurrentUnit = product.CurrentUnit - item.ProductUnit.Value;
                        if(product.CurrentUnit < 0)
                        {
                            return Ok(ResponseModel.Fail("建立失敗", null, 0, ""));
                        }
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
        public IActionResult getAllShipmentOrderNumber()
        {
            var orderNumber = (from a in _context.ShipmentOrders
                                select new { 
                                 Id = a.Id,
                                 Number = a.Number
                                }).ToList();
            return Ok(ResponseModel.Success("", orderNumber));
        }
        [HttpGet]
        public IActionResult getShipmentOrderById(int ShipmentOrderId)
        {
            var Order = _context.ShipmentOrders.FirstOrDefault(x => x.Id == ShipmentOrderId);
            var OrderDetails = (from a in _context.ShipmentOrderDetails
                                where a.OrderId == ShipmentOrderId
                                select a).ToList();
            if(Order == null)
            {
                return Ok(ResponseModel.Fail("找不到訂單資料", null, 0, ""));
            }
            else
            {
                return Ok(ResponseModel.Success("", new ShipmentOrderCreateViewModel
                {
                    ShipmentOrder = Order,
                    ShipmentOrderDetails = OrderDetails
                }));
            }
        }

        [HttpGet]
        public IActionResult ReturnShipmentOrderCreate()
        {                                       
            return View();
        }
        [HttpGet]
        public IActionResult ReturnShipmentOrderEdit(int ReturnShipmentOrderId)
        {
            ViewData["ReturnShipmentOrder"] = JsonConvert.SerializeObject(
                (from a in _context.ReturnShipmentOrders
                 where a.Id == ReturnShipmentOrderId
                 join b in _context.ShipmentOrders
                 on a.ShipmentOrderId equals b.Id
                 select new
                 {
                    Id = a.Id,
                    Number = a.Number,
                    ShipmentOrderId = a.ShipmentOrderId,
                    Total =a.Total,
                    Price =a.Price,
                    ReturnDate = a.ReturnDate,
                    Remarks = a.Remarks,
                    ShipmentOrderNumber = b.Number
                    }).FirstOrDefault()
               );

            ViewData["ReturnShipmentOrderDetails"] = JsonConvert.SerializeObject((
                from a in _context.ReturnShipmentOrderDetails
                where a.ReturnShipmentOrderId == ReturnShipmentOrderId
                join b in _context.ShipmentOrderDetails
                on a.ShipmentOrderDetailId equals b.Id
                select new {
                    Id = a.Id,
                    Remarks = a.Remarks,
                    ReturnShipmentOrderId = a.ReturnShipmentOrderId,
                    ShipmentOrderDetailId = a.ShipmentOrderDetailId,
                    Unit = a.Unit,
                    ProductName = b.ProductName,
                    ProductNumber = b.ProductNumber,
                    ProductUnit = b.ProductUnit,
                    ProductPrice = b.ProductPrice,
                    ProductId = b.ProductId,
                }).ToList());
            return View();
        }
        [HttpGet]
        public IActionResult ShipmentOrderEdit(int OrderId)
        {
            ViewData["Order"] = JsonConvert.SerializeObject(_context.ShipmentOrders.FirstOrDefault(x => x.Id == OrderId));
            ViewData["OrderDetails"] = JsonConvert.SerializeObject((from a in _context.ShipmentOrderDetails
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
