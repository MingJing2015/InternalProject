using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternalProject.Models
{
    public class RegisterUserVM
    {
        [Required]
        [Display(Name = "User name")]
        public string userName { get; set; }
        
        [Display(Name = "Vendor name")]
        public string vendorName { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string lastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "This is not a valid email address.")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Phone(ErrorMessage = "This is not a valid phone number.")]
        [Display(Name = "Phone")]
        public string phone { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Invalid Password.")]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        [Compare("password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
    }
}