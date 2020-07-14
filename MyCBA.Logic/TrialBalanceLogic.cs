using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class TrialBalanceLogic
    {
        TrialBalanceRepository trialRepo = new TrialBalanceRepository();

        public List<Transaction> GetTrialBalanceTransactions(DateTime startDate, DateTime endDate)
        {
            return trialRepo.GetTrialBalanceTransactions(startDate, endDate);
        }
    }
}
