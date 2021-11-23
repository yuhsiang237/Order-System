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
            RuleFor(x => x.OrderDetails).NotNull().WithMessage("請增加明細表內容");
            RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailsValidator());
        }
        public class OrderDetailsValidator : AbstractValidator<OrderDetail>
        {
            public OrderDetailsValidator()
            {
                RuleFor(x => x.ProductId).NotNull().WithMessage("必須選擇商品");
                RuleFor(x => x.ProductUnit).NotNull().WithMessage("必須填寫數量");
            }
        }
    }
}
