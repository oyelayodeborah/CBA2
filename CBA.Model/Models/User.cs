using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class User
    {
        public int id { get; set; }

        [Required]
        [StringLength(225)]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Full name should only contain characters and white spaces")]
        [Display(Name = "Full Name")]
        public string fullName { get; set; }


        //public Branch Branch { get; set; }
        //public Role Role { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        [StringLength(225)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [StringLength(225)]
        [Display(Name = "Password")]
        public string passwordHash { get; set; }

        [Required]
        [StringLength(11)]
        [MinLength(11)]
        [Display(Name = "Telephone Number")]
        public string phoneNumber { get; set; }

        public Role role { get; set; }
        [Required]
        [Display(Name = "Role")]
        public int roleId { get; set; }

        public Branch branch { get; set; }
        [Required]
        [Display(Name = "Branch")]
        public int branchId { get; set; }

        public string IsAssigned { get; set; }

        public string LoggedIn { get; set; }
    }
}
