using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class ShipmentOrderUpdateValidator : AbstractValidator<ShipmentOrderUpdateViewModel>
    {
        public ShipmentOrderUpdateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.ShipmentOrder.DeliveryDate).NotNull().WithMessage("出貨日期不可為空");
            RuleFor(x => x.ShipmentOrder.Address).NotNull().WithMessage("地址不可為空");

        }
    }
}

