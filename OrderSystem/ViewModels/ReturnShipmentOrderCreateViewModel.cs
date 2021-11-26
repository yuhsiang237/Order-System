using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class ReturnShipmentOrderCreateViewModel
    {
        public ReturnShipmentOrder ReturnShipmentOrder { get; set; }
        public List<ReturnShipmentOrderDetail> ReturnShipmentOrderDetails { get; set; }
    }
}
