﻿using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class ShipmentOrderViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int? Type { get; set; }
        public decimal? Total { get; set; }
        public string SignName { get; set; }
        public int? Status { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreateAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
