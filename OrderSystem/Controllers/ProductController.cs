using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Commons;
using OrderSystem.Models;
using OrderSystem.Models.Validator;
using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Controllers
{


    public class ProductController : Controller
    {
    
            public async Task<IActionResult> ProductCategory(
    string sortOrder,
    string currentFilterName,
    string searchStringName,
    int? goToPageNumber,
    int pageSize,
    int? pageNumber)
        {
            // 1.search logic
            var query = from a in _context.ProductCategories
                        where a.IsDeleted != true
                        select new ProductCategoryViewModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description
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
            return View(await PaginatedList<ProductCategoryViewModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

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

            var category = _context.ProductCategories.Where(x => x.IsDeleted != true)
                .Select(x => new { Id = x.Id,Name = x.Name });

            // 1.search logic
            var query = from a in _context.Products
                        where a.IsDeleted != true
                        select new ProductIndexViewModel
                        {
                            Id = a.Id,
                            Number = a.Number,
                            Name = a.Name,
                            CurrentUnit = a.CurrentUnit,
                            Price = a.Price,
                            Description = a.Description,
                            Category = (from b in _context.ProductProductCategoryRelationships
                                            join c in category
                                            on b.ProductCategoryId equals c.Id
                                            where b.ProductId == a.Id
                                            select new ProductCategory
                                            { 
                                                Id = c.Id,
                                                Name = c.Name
                                            }).ToList()
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
        public IActionResult ProductCategoryCreate(ProductCategory m)
        {
            // vaildate data
            ProductCategoryCreateValidator validator = new ProductCategoryCreateValidator(_context);
            ValidationResult result = validator.Validate(m);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
            }
            // add data
            ProductCategory pc = new ProductCategory();
            pc.Name = m.Name;
            pc.Description = m.Description;
            _context.ProductCategories.Add(pc);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }
        [HttpPost]
        public IActionResult ProductCategoryUpdate(ProductCategory m)
        {
            // vaildate data
            ProductCategoryUpdateValidator validator = new ProductCategoryUpdateValidator(_context);
            ValidationResult result = validator.Validate(m);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
            }
            // update data            
            ProductCategory pc = _context.ProductCategories.FirstOrDefault(x => x.Id == m.Id);
            pc.Name = m.Name;
            pc.Description = m.Description;
            _context.Update(pc);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }


        [HttpPost]
        public IActionResult DeleteProduct(Product model)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == model.Id);
            p.IsDeleted = true;
            _context.Update(p);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }
        [HttpPost]
        public IActionResult DeleteProductCategory(Product model)
        {
            var p = _context.ProductCategories.FirstOrDefault(x => x.Id == model.Id);
            p.IsDeleted = true;
            _context.Update(p);
            _context.SaveChanges();
            return Ok(ResponseModel.Success(""));
        }


        [HttpPost]

        public IActionResult UpdateProductUnit(Product model)
        {
            // vaildate data
            Dictionary<string, string[]> Errors = new Dictionary<string, string[]>();
            if (model.CurrentUnit == null || model.CurrentUnit < 0)
            {
                Errors.Add("CurrentUnit", new string[] { "數量不可小於0或為空" });
            }
            if (Errors.Count() > 0)
            {
                return Ok(ResponseModel.Fail(null, null, 0, Errors));
            }
            // data update
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var product = _context.Products.FirstOrDefault(x => x.Id == model.Id);
                    var inventoryUnit = model.CurrentUnit - product.CurrentUnit; // diff
                    product.CurrentUnit = model.CurrentUnit;
                    _context.Update(product);
                    ProductInventory productInventory = new ProductInventory();
                    productInventory.ProductId = product.Id;
                    productInventory.Unit = inventoryUnit;
                    productInventory.CreatedAt = DateTime.Now;
                    productInventory.Description = Constant.ProductInventoryChangeCode.ManualModify+":"+model.Description;
                    _context.ProductInventories.Add(productInventory);
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

        public IActionResult CreateProduct(CreateProductViewModel model)
        {

            // vaildate data
            CreateProductValidator validator = new CreateProductValidator(_context);
            ValidationResult result = validator.Validate(model);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
            }
            // data add
            using (var tr = _context.Database.BeginTransaction())
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
                    if (model.ProductCategory != null)
                    {
                        // add product category relationship
                        foreach (var item in model.ProductCategory)
                        {
                            ProductProductCategoryRelationship relationship = new ProductProductCategoryRelationship();
                            relationship.ProductCategoryId = item.Id;
                            relationship.ProductId = p.Id;
                            _context.ProductProductCategoryRelationships.Add(relationship);
                            _context.SaveChanges();
                        }
                    }
                    // create product inventory
                    ProductInventory pi = new ProductInventory();
                    pi.ProductId = p.Id;
                    pi.Unit = model.CurrentUnit;
                    pi.CreatedAt = DateTime.Now;
                    pi.Description = Constant.ProductInventoryChangeCode.Create;
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


        [HttpGet]
        public IActionResult GetAllProductCategory()
        {
            var result = (from a in _context.ProductCategories
                               where a.IsDeleted != true
                               select new
                               {
                                   Id = a.Id,
                                   Name = a.Name
                               }).ToList();
            return Ok(ResponseModel.Success("", result));
        }

        [HttpPost]

        public IActionResult UpdateProduct(UpdateProductViewModel model)
        {

            // vaildate data
            UpdateProductValidator validator = new UpdateProductValidator(_context);
            ValidationResult result = validator.Validate(model);
            if (!result.IsValid)
            {
                return Ok(ResponseModel.Fail(null, null, 0, result.Errors));
            }
            // update product basic info
            var p = _context.Products.FirstOrDefault(x => x.Id == model.Id);
            p.Name = model.Name;
            p.Description = model.Description;
            p.Price = model.Price;
            _context.Update(p);
            _context.SaveChanges();

            return Ok(ResponseModel.Success("", p));
        }
    }
}
