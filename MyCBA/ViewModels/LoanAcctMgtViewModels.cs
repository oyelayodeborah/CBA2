using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class LoanAcctMgtViewModels
    {

        public IEnumerable<GlAccount> GlAccounts { get; set; }

        public int id { get; set; }

        //[Required]
        [Display(Name = "Debit Interest Rate")]
        public decimal DebitInterestRate { get; set; }

        //[Required]
        [Display(Name = "Interest Income GL Account")]
        public string LoanInterestIncomeGL { get; set; }
        

    }
}
