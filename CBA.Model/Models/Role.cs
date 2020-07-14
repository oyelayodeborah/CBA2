using System;
using System.ComponentModel.DataAnnotations;

namespace MyCBA.Core.Models
{
    public class Role
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        [StringLength(30, ErrorMessage = "Pls Must be more than 3 letters and less than 31 letters", MinimumLength = 4)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Pls No Special Characters or numbers allowed")]
        public string name { get; set; }
    }
}
