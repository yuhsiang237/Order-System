using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class ReturnShipmentOrderViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public string ShipmentOrderNumber { get; set; }

        public decimal? Total { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
