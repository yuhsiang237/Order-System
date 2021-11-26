using OrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class ReturnShipmentOrderUpdateValidator : AbstractValidator<ReturnShipmentOrderCreateViewModel>
    {
        public ReturnShipmentOrderUpdateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.ReturnShipmentOrder.ReturnDate).NotNull().WithMessage("退貨日期不可為空");
            RuleFor(x => x.ReturnShipmentOrder.ShipmentOrderId).NotNull().WithMessage("出貨單資料錯誤");
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

