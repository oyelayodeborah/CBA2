using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class GlPostingViewModels
    {

        public IEnumerable<GlAccount> GlAccounts { get; set; }
        public GlAccount GlAccount { get; set; }

        public int id { get; set; }

        [Required(ErrorMessage = ("GL Account To Debit is required"))]
        [Display(Name = "GL Account To Debit")]
        public int GlAccountToDebit { get; set; }

        [Required(ErrorMessage = ("GL Account To Debit Code is required"))]
        public long GlAccountToDebitCode { get; set; }

        [Required(ErrorMessage = ("GL Account To Credit is required"))]
        [Display(Name = "GL Account To Credit")]
        public int GlAccountToCredit { get; set; }

        [Required(ErrorMessage = ("GL Account To Credit Code is required"))] 
        public long GlAccountToCreditCode { get; set; }

        [Required(ErrorMessage = ("Amount to be credited is required"))]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

    }
}
