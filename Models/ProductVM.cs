using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class ProductVM
    {
        [Display(Name = "ID")]
        public int productID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public string manufacturer { get; set; }
        
        [Display(Name = "Vendor")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string vendor { get; set; }

        [Required]
        [Display(Name = "Summary")]
        public string summary { get; set; }

        [Required]
        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        [Range(0, int.MaxValue, ErrorMessage = "A value bigger than 0 is needed.")]
        public decimal price { get; set; }

        [Required]
        [Display(Name = "Discounted Price")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        [Range(0, int.MaxValue, ErrorMessage = "A value bigger than 0 is needed.")]
        public decimal discountPrice { get; set; }

        [Required]
        [Display(Name = "Add Image")]
        public string imgUrl { get; set; }
    }
}