using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class ProductCategoryUpdateValidator : AbstractValidator<ProductCategory>
    {
        public ProductCategoryUpdateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x.Id).NotNull().WithMessage("分類發生錯誤");
            RuleFor(x => x).Custom((x, c) =>
            {
                var productCategory = context.ProductCategories.
                Where(x=>x.IsDeleted != true).
                FirstOrDefault(item =>
                    (item.Name == x.Name && item.Id != x.Id)
                    
                );
                if (productCategory != null)
                {
                    c.AddFailure("Name", "已有相同名稱");
                }
            });
        }
       
    }
}

