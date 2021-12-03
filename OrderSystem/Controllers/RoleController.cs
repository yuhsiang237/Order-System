using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleCreateViewModel m)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    RoleCreateValidator validator = new RoleCreateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // add return order & return order details
                    Role role = new Role();
                    role.Name = m.Role.Name;
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                    if (m.Permissions != null)
                    {
                        foreach (var item in m.Permissions)
                        {
                            item.RoleId = role.Id;
                            item.Code = item.Code;
                            _context.Permissions.Add(item);
                        }
                        _context.SaveChanges();
                    }
                    
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
        public IActionResult DeleteRole(Role model)
        {
            var p = _context.Roles.FirstOrDefault(x => x.Id == model.Id);
            p.IsDeleted = true;
            _context.Update(p);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }
        [HttpGet]
        public IActionResult RoleEdit(int RoleId)
        {
            ViewData["Role"] = JsonConvert.SerializeObject((from a in _context.Roles
                                                                   where a.Id == RoleId
                                                                   select a).FirstOrDefault());
            ViewData["Permissions"] = JsonConvert.SerializeObject((from a in _context.Permissions
                                                                   where a.RoleId == RoleId
                                                            select a).ToList());
            return View();
        }
        [HttpPost]
        public IActionResult RoleUpdate(RoleUpdateViewModel m)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    RoleUpdateValidator validator = new RoleUpdateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // add return order & return order details
                    Role role = _context.Roles.FirstOrDefault(x => x.Id == m.Role.Id);
                    role.Name = m.Role.Name;
                    _context.Update(role);
                    _context.SaveChanges();
                    // remove old permission
                    var removeOldPermission = (from a in _context.Permissions
                                                where a.RoleId == role.Id
                                                select a).FirstOrDefault();
                    if(removeOldPermission != null)
                    {
                        _context.Permissions.Remove(removeOldPermission);
                        _context.SaveChanges();
                    }
                    // add new permission
                    if (m.Permissions != null)
                    {
                        foreach (var item in m.Permissions)
                        {
                            item.RoleId = role.Id;
                            item.Code = item.Code;
                            _context.Permissions.Add(item);
                        }
                        _context.SaveChanges();
                    }
                    tr.Commit();
                    return Ok(ResponseModel.Success(""));
                }
                catch (Exception ex)
                {
                    return Ok(ResponseModel.Fail("建立失敗", null, 0, ""));
                }
            }

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
