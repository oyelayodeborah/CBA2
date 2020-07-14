using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class TellerPostingViewModels
    {
        public enum PostingType
        {
            Deposit=1,Withdrawal
        }

        public IEnumerable<CustomerAccount> CustomerAccount { get; set; }


        public int id { get; set; }

        public int customerId { get; set; }

        [Required]
        [Display(Name="Customer Name")]
        public string customerName { get; set; }

        [Required]
        [Display(Name = "Customer Account Number")]
        public long customerAcctNum { get; set; }

        [Required]
        [Display(Name = "Posting Type")]
        public PostingType postingType { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        [Display(Name = "Narration")]
        public string Narration { get; set; }
    }
}
