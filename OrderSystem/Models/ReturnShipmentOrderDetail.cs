using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystem.Models
{
    public partial class ReturnShipmentOrderDetail
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public int? ReturnShipmentOrderId { get; set; }
        public int? ShipmentOrderDetailId { get; set; }
        public decimal? Unit { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
