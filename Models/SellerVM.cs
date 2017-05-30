using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class SellerVM
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string firstName{ get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lastName{ get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "This is not a valid email address.")]
        [Display(Name = "Email")]
        public string email{ get; set; }

        [Required]
        [Phone(ErrorMessage = "This must be a number.")]
        [Display(Name = "Phone")]
        public string phone{ get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public string vendor { get; set; }

        // for display
        [Display(Name = "Address")]
        public string address { get; set; }
        // for edit
        public int addressID { get; set; }
        [Required]
        [Display(Name = "House Number")]
        public string addressHouseNumber { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string addressStreet { get; set; }

        [Required]
        [Display(Name = "City")]
        public string addressCity { get; set; }

        [Required]
        [Display(Name = "Province")]
        public string addressProvince { get; set; }
    }
}