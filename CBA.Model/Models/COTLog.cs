using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class COTLog
    {
        public int id { get; set; }

        
        public int customerId { get; set; }

        public long customerAcctNum { get; set; }

        public int GlAccountToCreditId { get; set; }

        public long GlAccountToCreditCode { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
