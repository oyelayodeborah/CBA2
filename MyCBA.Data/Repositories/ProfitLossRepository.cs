using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class ProfitLossRepository
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        DateTime yesterday;
        public ProfitLossRepository()
        {
            var getConfig = _context.AccountConfigurations.ToList().Count();
            if (getConfig != 0)
            {
                yesterday = _context.AccountConfigurations.First().FinancialDate.AddDays(-1); //since EOD there is no Expense/Income entries for today until EOD is run

            }
            else
            {
                yesterday = DateTime.Now;
            }
        }
        public List<ExpensesIncomeEntry> GetAllExpenseIncomeEntries()
        {
            return _context.ExpensesIncomeEntries.ToList();
        }
        public List<ExpensesIncomeEntry> GetEntries()
        {
            var result = new List<ExpensesIncomeEntry>();
            var allEntries = GetAllExpenseIncomeEntries();
            foreach (var item in allEntries)
            {
                var date=item.Date;
                if (date.Date == yesterday.Date)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<ExpensesIncomeEntry> GetEntries(DateTime startDate, DateTime endDate)
        {
            var result = new List<ExpensesIncomeEntry>();
            if (startDate < endDate)
            {
                //gets all entries(with their balances) for the start and the end dates. eg: Current exp gl (bal: 3k) on Jan5, (bal 8k) on Jan 9. etc. A GL cant exist more than 2 times (for start and end dates).
                var allEntries = GetAllExpenseIncomeEntries();
                foreach (var item in allEntries)
                {
                    if (item.Date.Date == startDate || item.Date.Date == endDate)
                    {
                        result.Add(item);
                    }
                }

            }
            return result.OrderByDescending(e => e.Date).ToList();  //making entries on endDate to come before those of startDate so that the difference in Account balance between the two days could be easily calculated
        }
    }
}
