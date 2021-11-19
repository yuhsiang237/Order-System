using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystem.Models
{
    public partial class ProductInventory
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public decimal? Unit { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
