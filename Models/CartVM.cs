using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class CartVM
    {
        public List<CartItemVM> Products { get; set; }
    }
}