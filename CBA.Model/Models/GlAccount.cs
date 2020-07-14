using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class GlAccount
    {
        public int id { get; set; }

        public GlCategory GlCategory { get; set; }

        [Required(ErrorMessage = ("GL Category is required"))]
        [Display(Name = "GL Category")]
        public int GlCategoryId { get; set; }

        [Required(ErrorMessage = ("Name is required")), MaxLength(40)]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Category name should only contain characters and white spaces")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = ("GL Account Code is required"))]
        [Display(Name = "GL Account Code")]
        public long Code {get; set; }

        public Branch Branch { get; set; }

        [Required(ErrorMessage = ("Branch is required"))]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name="Account Balance")]
        public decimal acountBalance { get; set; }

        public string assignToTeller { get; set; }

        [Display(Name="Main Category")]
        public string mainCategory { get; set; }
    }
}
