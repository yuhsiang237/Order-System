using System;

namespace OrderSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int Type { get; set; }
        public decimal? Total { get; set; }
        public string SignName { get; set; }
        public int? Status { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Boolean? IsDeleted { get; set; }

    }
}
