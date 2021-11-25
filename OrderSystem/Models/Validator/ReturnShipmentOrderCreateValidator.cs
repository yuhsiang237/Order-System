using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class ReturnShipmentOrderCreateValidator : AbstractValidator<ReturnShipmentOrderCreateViewModel>
    {
        public ReturnShipmentOrderCreateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.ReturnShipmentOrder.ReturnDate).NotNull().WithMessage("退貨日期不可為空");
            RuleFor(x => x.ReturnShipmentOrder).Custom((x, c) =>
            {
                var rso = context.ReturnShipmentOrders.FirstOrDefault(item => item.ShipmentOrderId == x.ShipmentOrderId);
                if (rso != null)
                {
                    c.AddFailure("ReturnShipmentOrder.ShipmentOrderId", "該出貨單已經建立過退貨單，請重新選擇");
                }
            });
            RuleFor(x => x.ReturnShipmentOrder.ShipmentOrderId).NotNull().WithMessage("必須選擇出貨單");
            RuleForEach(x => x.ReturnShipmentOrderDetails).SetValidator(new ReturnShipmentOrderDetailValidator(context));
        }
        public class ReturnShipmentOrderDetailValidator : AbstractValidator<ReturnShipmentOrderDetail>
        {
            public ReturnShipmentOrderDetailValidator(OrderSystemContext context)
            {
                RuleFor(x => x).Custom((x, c) =>
                {
                    var shipmentOrderDetail = context.ShipmentOrderDetails.FirstOrDefault(item => item.Id == x.ShipmentOrderDetailId);
                    if(shipmentOrderDetail!=null)
                    {
                        if (shipmentOrderDetail.ProductUnit < x.Unit)
                        {
                            c.AddFailure("Unit", "退貨數量不可超過出貨數量:" + shipmentOrderDetail.ProductUnit);
                        }
                    }
                    else
                    {
                        c.AddFailure("ShipmentOrderDetailId", "錯誤的訂單明細");

                    }
                });
            }
        }
    }
}

