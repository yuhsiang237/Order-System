using System;

namespace OrderSystem.ViewModels
{
    public class InventoryHistoryViewModel
    {
        public DateTime? CreateAt { get; set; }
        public Decimal? Unit { get; set; }
        public String? Descrption { get; set; }
    }
}
