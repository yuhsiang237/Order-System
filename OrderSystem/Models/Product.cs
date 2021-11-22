using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystem.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public decimal? CurrentUnit { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
