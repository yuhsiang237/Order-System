using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class ProductCategoryCreateValidator : AbstractValidator<ProductCategory>
    {
        public ProductCategoryCreateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x).Custom((x, c) =>
            {
                var productCategory = context.ProductCategories
                .Where(x => x.IsDeleted != true)
                .FirstOrDefault(item => item.Name == x.Name);
                if (productCategory != null)
                {
                    c.AddFailure("Name", "已有相同名稱");
                }
            });
        }
       
    }
}

