using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InternalProject.Models
{
    public class CheckoutVM
    {
        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Summary")]
        public string summary { get; set; }


        [Display(Name = "Subtotal")]
        public decimal subtotal { get; set; }

        [Display(Name = "GST")]
        public decimal gst { get; set; }

        [Display(Name = "PST")]
        public decimal pst { get; set; }

        [Display(Name = "Total")]
        public decimal total { get; set; }

        public List<CartItemVM> products { get; set; }

        // TODO : do not need if we use paypal
        [Display(Name = "Phone")]
        public string phone { get; set; }
        [Display(Name = "Credit Card")]
        public string ccard { get; set; }
        [Display(Name ="Expiration Date")]
        public string exp { get; set; }
    }
}