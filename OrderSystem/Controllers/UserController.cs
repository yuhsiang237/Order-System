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

    public class UserController : Controller
    {
        private readonly OrderSystemContext _context;

        public UserController( OrderSystemContext context)
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


        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignInAsync(UserSignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // vaild account and pwd
            var user = _context.Users.FirstOrDefault(x =>
               x.Account == model.Account
            );
            if (user == null)
            {
                ViewData["ErrorAccount"] = "錯誤的帳號或密碼";
                return View();
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
               return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["ErrorAccount"] = "錯誤的帳號或密碼";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");// direct to home page
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
                return View();
            }
            if (isExistEmail != null)
            {
                ViewData["ErrorEmailExist"] = "信箱已存在";
                return View();
            }
            if (model.Password != model.Password2)
            {
                ViewData["ErrorPassword2"] = "密碼輸入不一致";
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            // create an account
            User user = new User();
            user.Name = model.Name;
            user.Email = model.Name;
            user.Account = model.Account;
            // Hash & Salt password
            var hashSaltResponse = HashSaltTool.Generate(model.Password);
            user.Password = hashSaltResponse.hash;
            user.Salt = hashSaltResponse.salt;

            _context.Users.Add(user);
            _context.SaveChanges();
            return View();
        }
    }
}
