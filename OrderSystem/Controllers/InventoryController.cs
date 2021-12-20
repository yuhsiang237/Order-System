using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Authorization;
using OrderSystem.Commons;
using OrderSystem.Models;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Controllers
{
     
    public class InventoryController : Controller
    {
        private readonly OrderSystemContext _context;



        public InventoryController(OrderSystemContext context)
        {
            _context = context;
        }

        public IActionResult InventoryHistory(int productId)
        {

            IEnumerable<InventoryHistoryViewModel> query = 
                        from a in _context.ProductInventories
                        where a.ProductId == productId
                        orderby a.Id descending
                        select new InventoryHistoryViewModel
                        {
                            CreateAt = a.CreatedAt,
                            Descrption = a.Description,
                            Unit = a.Unit
                        };
            Product product =
                        (from a in _context.Products
                        where a.Id == productId
                        select new Product
                        {
                            Name = a.Name,
                            Number = a.Number,
                            CurrentUnit = a.CurrentUnit,
                            Price = a.Price
                        }).FirstOrDefault(); ;

            ViewData["Name"] = product.Name;
            ViewData["Number"] = product.Number;
            ViewData["CurrentUnit"] = product.CurrentUnit;
            ViewData["Price"] = product.Price;

            return View(query);
        }
        [PermissionFilter(Permissions.Inventory_View)]
        [HttpGet]

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
                        where a.IsDeleted != true
                        select new InventoryIndexViewModel
                        {
                            Id = a.Id,
                            Number = a.Number,
                            Name = a.Name,
                            CurrentUnit = a.CurrentUnit,
                            Price = a.Price
                        };

            // 2.condition filter
            if (searchStringName != null || searchStringNumber != null)
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
            return View(await PaginatedList<InventoryIndexViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
