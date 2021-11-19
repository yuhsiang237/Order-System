using System;
using System.Collections.Generic;


namespace OrderSystem.Models
{
    public partial class ProductProductCategoryRelationship
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductCategoryId { get; set; }
    }
}
