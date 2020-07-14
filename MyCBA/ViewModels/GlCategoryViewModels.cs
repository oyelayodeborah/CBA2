using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public enum MainAccountCategory
    {
        Asset = 1, Capital, Expenses, Income, Liability
    }
    public class GlCategoryViewModels
    {
        

        public GlCategory GlCategories { get; set; }
        public int id { get; set; }

        [Display(Name = "Name")]
        public string name { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Main Account Category")]
        public MainAccountCategory mainAccountCategory { get; set; }

       

    }
}
