using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductViewModel>
    {
        public UpdateProductValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x.Price).NotNull().WithMessage("價格不可為空")
                                 .GreaterThanOrEqualTo(0).WithMessage("價格必須大於等於0");
        }

    }
}

