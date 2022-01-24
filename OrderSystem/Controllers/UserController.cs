using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

    public class UserController : Controller
    {
        private readonly OrderSystemContext _context;
        public UserController( OrderSystemContext context)
        {
            _context = context;
        }

        [PermissionFilter(Permissions.Basic_UserManagement_View)]
        [HttpGet]
        public async Task<IActionResult> Search(
     string sortOrder,
     string currentFilterName,
     string searchStringName,
     int? goToPageNumber,
     int pageSize,
     int? pageNumber)
        {
            // 1.search logic
            var query = from a in _context.Users
                        where a.IsDeleted != true
                        select new UserIndexViewModel
                        {
                            Id = a.Id,
                            Account = a.Account,
                            Name = a.Name,
                            Email = a.Email
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
                    query = query.OrderByDescending(s => s.Account);
                    break;
                case "2":
                    query = query.OrderBy(s => s.Account);
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
            return View(await PaginatedList<UserIndexViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]

        public IActionResult UpdateUser(int UserId)
        {
            ViewData["User"] = JsonConvert.SerializeObject((from a in _context.Users
                                                            where a.Id == UserId
                                                            join b in _context.Roles
                                                            on a.RoleId equals b.Id
                                                            select new { 
                                                             Id = a.Id,
                                                             Account = a.Account,
                                                             RoleId = a.RoleId,
                                                             Name = a.Name,
                                                             Email = a.Email,
                                                             RoleName = b.Name
                                                            }).FirstOrDefault());


            // update
            string userAccount = null;
            //get user permission
            foreach (Claim claim in User.Claims)
            {
                if (claim.Type == "Account")
                {
                    userAccount = claim.Value;
                    break;
                }
            }
            User user = _context.Users.FirstOrDefault(x => x.Account == userAccount);
       
            // role character modify permission
            var isRoleModifyPermission = _context.Permissions.
        FirstOrDefault(item => item.RoleId == user.RoleId
        && item.Code == Permissions.Basic_Permission_Modify);
            if (isRoleModifyPermission != null)
            {
                ViewData["Roles"] = JsonConvert.SerializeObject((from a in _context.Roles
                                                                 where a.IsDeleted != true                                        select a).ToList());
            }
            else
            {
                ViewData["Roles"] = JsonConvert.SerializeObject((from a in _context.Roles
                                                                 where a.Id == user.RoleId
                                                                 select a).ToList());
            }
            
            return View();
        }

        [HttpGet]

        public IActionResult UpdateUserSelf()
        {
            string userAccount = null;
            foreach (Claim claim in User.Claims)
            {
                if (claim.Type == "Account")
                {
                    userAccount = claim.Value;
                    break;
                }
            }

            ViewData["User"] = JsonConvert.SerializeObject((from a in _context.Users
                                                            where a.Account == userAccount
                                                            join b in _context.Roles
                                                            on a.RoleId equals b.Id
                                                            select new
                                                            {
                                                                Id = a.Id,
                                                                Account = a.Account,
                                                                RoleId = a.RoleId,
                                                                Name = a.Name,
                                                                Email = a.Email,
                                                                RoleName = b.Name
                                                            }).FirstOrDefault());


          
            //get user permission
            User user = _context.Users.FirstOrDefault(x => x.Account == userAccount);

            // role character modify permission
            var isRoleModifyPermission = _context.Permissions.
        FirstOrDefault(item => item.RoleId == user.RoleId
        && item.Code == Permissions.Basic_Permission_Modify);
            if (isRoleModifyPermission != null)
            {
                ViewData["Roles"] = JsonConvert.SerializeObject((from a in _context.Roles
                                                                 where a.IsDeleted != true
                                                                 select a).ToList());
            }
            else
            {
                ViewData["Roles"] = JsonConvert.SerializeObject((from a in _context.Roles
                                                                 where a.Id == user.RoleId
                                                                 select a).ToList());
            }

            return View();
        }

        [HttpPost]
        [PermissionFilter(Permissions.Basic_UserManagement_Delete)]
        public IActionResult DeleteUser(Product model)
        {
            var p = _context.Users.FirstOrDefault(x => x.Id == model.Id);
            p.IsDeleted = true;
            _context.Update(p);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }
        public IActionResult SignUp()
        {
            return PartialView();
        }

        [HttpPost]
        [PermissionFilter(Permissions.Basic_UserManagement_Modify)]
        public IActionResult UpdateUser(UserUpdateViewModel m)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    UserUpdateValidator validator = new UserUpdateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // update
                    User user = _context.Users.FirstOrDefault(x => x.Id == m.Id);
                    user.Name = m.Name;
                    user.Email = m.Email;
                    // update role character
                    if (user.RoleId != m.RoleId)
                    {
                        var isRoleModifyPermission = _context.Permissions.
                    FirstOrDefault(item => item.RoleId == user.RoleId
                    && item.Code == Permissions.Basic_Permission_Modify);
                        if (isRoleModifyPermission != null)
                        {
                            user.RoleId = m.RoleId;
                        }
                    }
                    _context.Update(user);
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
        [PermissionFilter(Permissions.Default_Login)]
        public IActionResult UpdateUserSelf(UserUpdateViewModel m)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    // vaildate data
                    UserUpdateValidator validator = new UserUpdateValidator(_context);
                    ValidationResult result = validator.Validate(m);
                    if (!result.IsValid)
                    {
                        return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
                    }
                    // update
                    User user = _context.Users.FirstOrDefault(x => x.Id == m.Id);
                    user.Name = m.Name;
                    user.Email = m.Email;
                    // update role character
                    if (user.RoleId != m.RoleId)
                    {
                        var isRoleModifyPermission = _context.Permissions.
                    FirstOrDefault(item => item.RoleId == user.RoleId
                    && item.Code == Permissions.Basic_Permission_Modify);
                        if (isRoleModifyPermission != null)
                        {
                            user.RoleId = m.RoleId;
                        }
                    }
                    _context.Update(user);
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


        public IActionResult SignIn()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SignInAsync(UserSignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView();
            }
            // vaild account and pwd
            var user = _context.Users.FirstOrDefault(x =>
               x.Account == model.Account
            );
            if (user == null)
            {
                ViewData["ErrorAccount"] = "錯誤的帳號或密碼";
                return PartialView();
            }
            Boolean isValid = HashSaltTool.Validate(model.Password, user.Salt, user.Password);
            if (isValid)
            {   // login success 
                Claim[] claims = new[] {
                    new Claim("Account", user.Account),
                    new Claim("ID",user.Id.ToString()) 
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                // cookie-based login
                await HttpContext.SignInAsync(claimsPrincipal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = true, // keep login when close browser
                    });
               return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewData["ErrorAccount"] = "錯誤的帳號或密碼";
                return PartialView();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn", "User");// direct to sign in page
        }

        [HttpPost]
        public IActionResult SignUp(UserSignUpViewModel model)
        {
            string account = model.Account;
            string email = model.Email;
            // validate data
            var isExistAccount = _context.Users.FirstOrDefault(x=>
                x.Account == account 
            );
            var isExistEmail = _context.Users.FirstOrDefault(x =>
              x.Email == email
            );
            if (isExistAccount != null)
            {
                ViewData["ErrorAccountExist"] = "帳號已存在";
                return PartialView();
            }
            if (isExistEmail != null)
            {
                ViewData["ErrorEmailExist"] = "信箱已存在";
                return PartialView();
            }
            if (model.Password != model.Password2)
            {
                ViewData["ErrorPassword2"] = "密碼輸入不一致";
                return PartialView();

            }
            if (!ModelState.IsValid)
            {
                return PartialView();

            }
            // create an account
            User user = new User();
            user.Name = model.Name;
            user.Email = model.Email;
            user.Account = model.Account;
            user.RoleId = 1; // basic role
            // Hash & Salt password
            var hashSaltResponse = HashSaltTool.Generate(model.Password);
            user.Password = hashSaltResponse.hash;
            user.Salt = hashSaltResponse.salt;

            _context.Users.Add(user);
            _context.SaveChanges();
            ViewData["IsSuccess"] = "true";
            ViewData["Account"] = model.Account;
            return PartialView();
        }
    }
}
