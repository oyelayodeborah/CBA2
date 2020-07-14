using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class TrialBalanceViewModels
    {
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
        public string AccountName { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
    }
}
