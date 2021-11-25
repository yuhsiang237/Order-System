using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class ShipmentOrderUpdateViewModel
    {
        public ShipmentOrder Order { get; set; }
        public List<ShipmentOrderDetail> OrderDetails { get; set; }
    }
}
