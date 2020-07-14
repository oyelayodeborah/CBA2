using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class ExpensesIncomeEntry
    {
        public enum PandLType
        {
            Income, Expenses
        }
        
            public int ID { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string AccountName { get; set; }
            public PandLType EntryType { get; set; }
        
    }
}
