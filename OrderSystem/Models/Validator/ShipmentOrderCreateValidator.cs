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
        public ShipmentOrderCreateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Order.DeliveryDate).NotNull().WithMessage("出貨日期不可為空");
            RuleFor(x => x.Order.Address).NotNull().WithMessage("地址不可為空");
            RuleFor(x => x.OrderDetails).NotNull().WithMessage("請增加明細表內容");
            RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailsValidator(context));
        }
        public class OrderDetailsValidator : AbstractValidator<OrderDetail>
        {
            public OrderDetailsValidator(OrderSystemContext context)
            {
                RuleFor(x => x.ProductId).NotNull().WithMessage("必須選擇商品");
                RuleFor(x => x.ProductUnit).NotNull().WithMessage("必須填寫數量");
                RuleFor(x => x).Custom((x, c) =>
                {
                    var product = context.Products.FirstOrDefault(item => item.Id == x.ProductId);
                    if (product == null)
                    {
                        c.AddFailure("ProductId", "商品資訊錯誤，請重選擇商品");
                    }
                    if ((product.CurrentUnit - x.ProductUnit) < 0)
                    {
                        c.AddFailure("ProductUnit", "庫存數量不足，目前庫存量:" + product.CurrentUnit);
                    }
                });
            }
        }
    }
}

