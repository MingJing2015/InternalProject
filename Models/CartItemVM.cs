using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class CartItemVM
    {
        public int productID { get; set; }

        [Display(Name = "Username")]
        public string userName { get; set; }

        [Display(Name = "Product Name")]
        public string productName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal price { get; set; }

        [Display(Name = "Discounted Price")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal discountPrice { get; set; }

        [Display(Name = "Quantity")]
        public int quantity { get; set; }
        public string imgUrl { get; set; }
    }
}