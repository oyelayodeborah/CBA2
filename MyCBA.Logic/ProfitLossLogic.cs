using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class ProfitLossLogic
    {
        ProfitLossRepository plRepo = new ProfitLossRepository();

        public List<ExpensesIncomeEntry> GetEntries()
        {
            var pl= plRepo.GetEntries();
            return pl;
        }
        public List<ExpensesIncomeEntry> GetAllExpenseIncomeEntries()
        {
            var pl = plRepo.GetAllExpenseIncomeEntries();
            return pl;
        }

        public List<ExpensesIncomeEntry> GetEntries(DateTime startDate, DateTime endDate)
        {
            var pl = plRepo.GetEntries(startDate, endDate);
            return pl;
        }
    }
}
