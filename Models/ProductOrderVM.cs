using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class ProductOrderVM
    {
        [Display(Name = "Product Name")]
        public string productName { get; set; }
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
    }
}