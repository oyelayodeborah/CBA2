using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public enum MainAccountCategory
    {
        Asset = 1, Capital, Expenses, Income, Liability
    }
    public class GlCategory
    {
        public int id { get; set; }

        [Required(ErrorMessage = ("Category name is required")), MaxLength(40)]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Category name should only contain characters and white spaces")]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string name { get; set; }

        public long code { get; set; }

        [Required(ErrorMessage = ("Please enter a description")), MaxLength(150)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Main Account Category")]
        [Required(ErrorMessage = "You have to select a main GL Category")]
        public MainAccountCategory mainAccountCategory { get; set; }

        //[Display(Name = "Sub Account Category")]
        //public string subAccountCategory { get; set; }
    }
}
