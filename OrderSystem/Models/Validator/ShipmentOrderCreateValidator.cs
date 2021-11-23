using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class ShipmentOrderCreateValidator : AbstractValidator<ShipmentOrderCreateViewModel>
    {

        public ShipmentOrderCreateValidator()
        {
            RuleFor(x => x.Order.DeliveryDate).NotNull().WithMessage("出貨日期不可為空");
        }

    }
}
