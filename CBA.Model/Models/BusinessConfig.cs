using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class BusinessConfig
    {
        public int id { get; set; }
        public DateTime FinancialDate { get; set; }
        public bool IsBusinessOpen { get; set; }

        public int DayCount { get; set; }
        public int MonthCount { get; set; }

        public int YearCount { get; set; }
        
    }
}
