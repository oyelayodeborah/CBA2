using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
        public class BalanceSheetLogic
        {
            BalancesheetRepository bsRepo = new BalancesheetRepository();

            public List<GlAccount> GetAssetAccounts()
            {
                return bsRepo.GetAssetAccounts();
            }
            
            public List<GlAccount> GetCapitalAccounts()
            {
                return bsRepo.GetCapitalAccounts();
            }

            public List<LiabilityViewModel> GetLiabilityAccounts()
            {
                return bsRepo.GetLiabilityAccounts();
            }
    }
}
