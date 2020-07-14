using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class TellerPosting
    {
       
        public int id { get; set; }

        public Customer customer { get; set; }
        [Required]
        [Display(Name = "Customer")]
        public int customerId { get; set; }

        [Required]
        [Display(Name = "Customer Account Number")]
        public long customerAcctNum { get; set; }

        [Required]
        [Display(Name = "Posting Type")]
        public string postingType { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string status { get; set; }

        public User user { get; set; }
        public int userId { get; set; }
    }
}
