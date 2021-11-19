using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Commons;
using OrderSystem.Models;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index(
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
            var query = from a in _context.Products
                        select new ProductIndexViewModel
                        {
                            Id = a.Id,
                            Number = a.Number,
                            Name = a.Name,
                            CurrentUnit = a.CurrentUnit,
                            Price = a.Price
                        };

            // 2.condition filter
            if (searchStringName!=null ||searchStringNumber != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchStringName = currentFilterName;
                searchStringNumber = currentFilterNumber;
            }

            ViewData["CurrentFilterNumber"] = searchStringNumber;
            ViewData["CurrentFilterName"] = searchStringName;

            if (!String.IsNullOrEmpty(searchStringNumber))
            {
                query = query.Where(s => s.Number.Contains(searchStringNumber));
            }
            if (!String.IsNullOrEmpty(searchStringName))
            {
                query = query.Where(s => s.Name.Contains(searchStringName));
            }
            // 3.sort data
            ViewData["CurrentSort"] = sortOrder;

            switch (sortOrder)
            {
                case "0":
                    query = query.OrderByDescending(s => s.Id);
                    break;
                case "1":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                case "2":
                    query = query.OrderBy(s => s.Name);

                    break;
                case "3":
                    query = query.OrderByDescending(s => s.CurrentUnit);

                    break;
                case "4":
                    query = query.OrderBy(s => s.CurrentUnit);

                    break;
                case "5":
                    query = query.OrderByDescending(s => s.Price);

                    break;
                case "6":
                    query = query.OrderBy(s => s.Price);

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
            return View(await PaginatedList<ProductIndexViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private readonly OrderSystemContext _context;

        public ProductController(OrderSystemContext context)
        {
            _context = context;
        }

        [HttpPost]

        public IActionResult CreateProduct(Product model)
        {

            // vaildate data
            Dictionary<string, string[]> Errors = new Dictionary<string, string[]>();
            if (model.Name == null || model.Name == "")
            {
                Errors.Add("Name", new string[] { "請輸入名稱" });
            }
            if (model.Number == null || model.Number == "")
            {
                Errors.Add("Number", new string[] { "請輸入編號" });
            }
            var isExistNumber = _context.Products.FirstOrDefault(x =>
                x.Number == model.Number
            );
            if (isExistNumber != null)
            {
                Errors.Add("Number", new string[] { "編號已存在，請更換名稱" });
            }
            if (model.Price == null)
            {
                Errors.Add("Price", new string[] { "請輸入價錢" });
            }
            if (model.Price < 0)
            {
                Errors.Add("Price", new string[] { "價錢不可為負" });
            }
            if (model.CurrentUnit == null)
            {
                Errors.Add("CurrentUnit", new string[] { "請輸入數量" });
            }
            if (model.CurrentUnit < 0)
            {
                Errors.Add("CurrentUnit", new string[] { "數量不可為負" });
            }
            if (Errors.Count() > 0)
            {
                return Ok(ResponseModel.Fail(null, null, 0, Errors));
            }
            // data add
            using(var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // create product
                    Product p = new Product();
                    p.Name = model.Name;
                    p.Number = model.Number;
                    p.CurrentUnit = model.CurrentUnit;
                    p.Description = model.Description;
                    p.Price = model.Price;
                    _context.Products.Add(p);
                    _context.SaveChanges();
                    // create product inventory
                    ProductInventory pi = new ProductInventory();
                    pi.ProductId = p.Id;
                    pi.Unit = model.CurrentUnit;
                    pi.Description = "新增商品初始化";
                    _context.ProductInventories.Add(pi);
                    _context.SaveChanges();

                    tr.Commit();
                    return Ok(ResponseModel.Success("", p));
                }
                catch (Exception ex)
                {
                    return Ok(ResponseModel.Fail("建立失敗", null, 0, ""));
                }
            }
        }
    }
}
