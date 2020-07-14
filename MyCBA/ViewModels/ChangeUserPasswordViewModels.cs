using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.ViewModels
{
    public class ChangeUserPasswordViewModels
    {
        public int id { get; set; }

        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string current_password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string new_password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("new_password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirm_password { get; set; }
    }
}
