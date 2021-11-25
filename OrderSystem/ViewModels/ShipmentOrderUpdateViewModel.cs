using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class ShipmentOrderUpdateViewModel
    {
        public ShipmentOrder ShipmentOrder { get; set; }
        public List<ShipmentOrderDetail> ShipmentOrderDetails { get; set; }
    }
}
