using Microsoft.AspNetCore.Mvc;
using OrderSystem.Helpers;
using OrderSystem.Models;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
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
            // todo hash or salt 
            user.Password = model.Password; 
            user.Email = model.Name;
            user.Account = model.Account;
        
            return View();
        }
    }
}
