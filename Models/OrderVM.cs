using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class OrderVM
    {
        [Display(Name = "Order ID")]
        public int orderId { get; set; }

        [Display(Name = "Time Stamp")]
        public DateTime? timeStamp { get; set; }

        [Display(Name = "Summary")]
        public List<ProductOrderVM> summary { get; set; }
    }
}