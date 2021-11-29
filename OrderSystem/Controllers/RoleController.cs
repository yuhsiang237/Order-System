using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Authorization;
using OrderSystem.Commons;
using OrderSystem.Models;
using OrderSystem.Tools;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderSystem.Controllers
{

    public class RoleController : Controller
    {
        private readonly OrderSystemContext _context;

        public RoleController( OrderSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
     string sortOrder,
     string currentFilterName,
     string searchStringName,
     int? goToPageNumber,
     int pageSize,
     int? pageNumber)
        {
            // 1.search logic
            var query = from a in _context.Roles
                        where a.IsDeleted != true
                        select new RoleIndexViewModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                        };

            // 2.condition filter
            if (searchStringName != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchStringName = currentFilterName;
            }

            ViewData["CurrentFilterName"] = searchStringName;

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
            return View(await PaginatedList<RoleIndexViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


    }
}
