using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class LoanCustAcctLogic
    {
        LoanCustAcctRepository loanRepo = new LoanCustAcctRepository();
        public LoanCustAcct LoanAccountExist(int servicingAcctId)
        {
            var getLoanAccount = loanRepo.GetAccount(servicingAcctId);
            if (getLoanAccount != null)
            {
                return getLoanAccount;
            }
            else
            {
                return null;
            }
        }
    }
}
