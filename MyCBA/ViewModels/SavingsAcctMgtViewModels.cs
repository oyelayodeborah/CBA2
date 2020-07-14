using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class SavingsAcctMgtViewModels
    {

        public IEnumerable<GlAccount> GlAccounts { get; set; }

        public int id { get; set; }

        //[Required]
        [Display(Name = "Credit Interest Rate")]
        public decimal CreditInterestRate { get; set; }

        [Display(Name = "Lien")]
        public decimal Lien { get; set; }

        //[Required]
        [Display(Name = "Minimum Balance")]
        public decimal MinBalance { get; set; }

        //[Required]
        [Display(Name = "Interest Expense GL Account")]
        public string SavingsInterestExpenseGL { get; set; }

    }
}
