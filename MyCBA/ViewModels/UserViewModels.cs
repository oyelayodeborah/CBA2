using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class UserViewModels
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        //public User User { get; set; }

        public int id { get; set; }

        [RegularExpression("^[A-Z\\sa-z]+$",ErrorMessage ="FullName should not contain special character or numbers")]
        [Required]
        [StringLength(225)]
        [Display(Name = "Full Name")]
        public string fullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(225)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address"), MaxLength(255)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [RegularExpression("^[A-Z\\sa-z]+$", ErrorMessage = "Username should not contain special character or numbers")]
        [Required]
        [StringLength(225)]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [RegularExpression("\\d+$", ErrorMessage = "Username should not contain special character or numbers")]
        [Phone]
        [Display(Name = "Telephone Number")]
        public string phoneNumber { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int roleId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int branchId { get; set; }

    }
}
