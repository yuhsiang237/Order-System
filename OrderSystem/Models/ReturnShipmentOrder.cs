using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystem.Models
{
    public partial class ReturnShipmentOrder
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int? ShipmentOrderId { get; set; }
        public decimal? Total { get; set; }
        public decimal? Price { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
