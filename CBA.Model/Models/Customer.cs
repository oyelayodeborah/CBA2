using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MyCBA.Core.Models
{
    public class Customer
    {
        public int id { get; set; }

        public string customerID { get; set; }

        [Required]
        [StringLength(225, ErrorMessage = "Customer's name can not be less than 5 letters", MinimumLength = 5)]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Full name should only contain characters and white spaces")]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [StringLength(225)]
        [Display(Name = "Address")]
        [MinLength(4)]
        public string address { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, ErrorMessage = "Telephone Number cannot be less than 11 letters", MinimumLength = 11)]
        [Display(Name = "Telephone Number")]
        public string phoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        [StringLength(225, ErrorMessage = "Email Address cannot be less than 9 letters", MinimumLength = 9)]
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string gender { get; set; }
    }
}