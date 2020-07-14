using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string AccountName { get; set; }
        public string SubCategory { get; set; }     //eg customerAccount, CashAsset etc
        public string MainCategory { get; set; }
        public string TransactionType { get; set; }
    }
}
