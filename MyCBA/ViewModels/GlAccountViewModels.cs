using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class GlAccountViewModels
    {
        public IEnumerable<GlCategory> GlCategories { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        public GlAccount GlAccount { get; set; }

        //public enum MainGlCategory
        //{
        //    Asset, Capital, Expenses, Income, Liability
        //}
        //public enum SubGlCategory
        //{
        //    Current, Fixed
        //}

        public int id { get; set; }

        //[Required(ErrorMessage = ("Main GL Category is required")), MaxLength(40)]
        [Display(Name = "Main GL Category")]
        public string glcategory { get; set; }

        //[Required(ErrorMessage = ("Sub GL Category is required")), MaxLength(40)]
        //[Display(Name = "Sub GL Category")]
        //public SubGlCategory subglcategory { get; set; }

        //[Required(ErrorMessage = ("Name is required")), MaxLength(40)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        //[Required(ErrorMessage = ("Branch is required")), MaxLength(100)]
        //[Display(Name = "Branch")]
        //public string BranchName { get; set; }

        [Display(Name = "Branch")]
        public int BranchId { get; set; }

    }
}
