using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ProductUnit { get; set; }
        public string Remarks { get; set; }
        Boolean? IsDeleted { get; set; }
    }
}
