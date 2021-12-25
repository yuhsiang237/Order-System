using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class CreateProductValidator : AbstractValidator<CreateProductViewModel>
    {
        public CreateProductValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x.Number).NotNull().WithMessage("編號不可為空");
            RuleFor(x => x.Price).NotNull().WithMessage("價格不可為空")
                                 .GreaterThanOrEqualTo(0).WithMessage("價格必須大於等於0");
            RuleFor(x => x.CurrentUnit).NotNull().WithMessage("數量不可為空").
                GreaterThanOrEqualTo(0).WithMessage("價格必須大於等於0");
            RuleFor(x => x).Custom((x, c) =>
            {
                var isExistNumber = context.Products.FirstOrDefault(item =>
                            item.IsDeleted !=true &&
                            item.Number == x.Number
                        );
                if (isExistNumber != null)
                {
                    c.AddFailure("Number", "已有相同編號，編號不可重複");
                }
            });
        }

    }
}

