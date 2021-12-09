using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{

    public class ProductIndexViewModel
    {

        public int Id { get; set; }

        [Display(Name = "商品編號")]

        public string Number { get; set; }

        [Display(Name = "商品分類")]

        public List<Models.ProductCategory> Category { get; set; }

        [Display(Name = "商品名稱")]

        public string Name { get; set; }
        [Display(Name = "商品價格")]

        public decimal? Price { get; set; }
        [Display(Name = "分類名稱")]

        public decimal? CurrentUnit { get; set; }
        public string Description { get; set; }
    }

}
