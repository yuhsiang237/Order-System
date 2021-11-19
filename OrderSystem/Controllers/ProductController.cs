using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
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
            Product m = new Product();
            m.Name = model.Name;
            m.Number = model.Number;
            m.CurrentUnit = model.CurrentUnit;
            m.Description = model.Description;
            m.Price = model.Price;
            _context.Products.Add(m);
            _context.SaveChanges();

            return Ok(ResponseModel.Success("", m));
        }
    }
}
