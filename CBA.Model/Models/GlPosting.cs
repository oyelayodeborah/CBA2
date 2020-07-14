using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class GlPosting
    {
        public int id { get; set; }

        public GlAccount GlAccountToDebit { get; set; }
        [Required(ErrorMessage = ("GL Account To Debit is required"))]
        [Display(Name = "GL Account To Debit")]
        public int GlAccountToDebitId { get; set; }

        [Required(ErrorMessage = ("GL Account To Debit Code is required"))]
        public long GlAccountToDebitCode { get; set; }

        public GlAccount GlAccountToCredit { get; set; }

        [Required(ErrorMessage = ("GL Account To Credit is required"))]
        [Display(Name = "GL Account To Credit")]
        public int GlAccountToCreditId { get; set; }

        [Required(ErrorMessage = ("GL Account To Credit Code is required"))] 
        public long GlAccountToCreditCode { get; set; }

        [Required(ErrorMessage = ("Amount to be credited is required"))]
        [Display(Name = "Amount")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Credit Amount must be between 0 and a maximum reasonable value")]
        public decimal Amount { get; set; }

        [Display(Name = "Narration")]
        [MaxLength(255,ErrorMessage ="Narration can not be more than 255 characters")]
        [MinLength(3,ErrorMessage = "Narration should be more than 3 characters")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Narration { get; set; }

        public User user { get; set; }

        public int userId { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Report")]
        public string report { get; set; }

        [Required(ErrorMessage ="Transaction Date is required")]
        public DateTime TransactionDate { get; set; }

        //[Required(ErrorMessage = "Financial Date is required")]
        //public DateTime FinancialDate { get; set; }
    }
}
