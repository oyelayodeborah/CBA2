using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class BalancesheetRepository
    {
        GlAccountRepository glactRepo = new GlAccountRepository();
        CustomerAccountRepository custRepo = new CustomerAccountRepository();
        LoanCustAcctRepository loanRepo = new LoanCustAcctRepository();

        public List<GlAccount> GetAssetAccounts()
        {
            GlCategoryRepository glCatRepo = new GlCategoryRepository();
            //var value= glactRepo.GetByMainCategory("Asset").RemoveAll(a=>a.GlCategoryId==getCatId.id);
            var allAssets = new List<GlAccount>();
            GlAccount cashAsset = new GlAccount();
            cashAsset.Name = "Cash";
            decimal cashAssetSum = glactRepo.GetAllCategoryName("Cash").Sum(a => a.acountBalance);
            cashAsset.acountBalance = cashAssetSum;
            allAssets = glactRepo.GetAllAsset().ToList();
            allAssets.Add(cashAsset);
            //GlAccount loanAsset = new GlAccount();
            //loanAsset.Name = "Loan Accounts";
            //var loanAccounts = loanRepo.GetAll();
            //var loanAccounts = custRepo.GetByType(AccountType.Loan);
            //foreach (var act in loanAccounts)
            //{
            //    loanAsset.acountBalance += act.LoanPrincipalRemaining;
            //}
            //allAssets.Add(loanAsset);
            return allAssets;
        }

        public List<GlAccount> GetCapitalAccounts()
        {
            var allCapitals = glactRepo.GetByMainCategory("Capital");
            //adding the "Reserves" capitals--> Profit or loss expressed as (Income - Expense)
            GlAccount reserveCapital = new GlAccount();
            reserveCapital.Name = "Reserves";
            decimal incomeSum = glactRepo.GetByMainCategory("Income").Sum(a => a.acountBalance);
            decimal expenseSum = glactRepo.GetByMainCategory("Expenses").Sum(a => a.acountBalance);
            reserveCapital.acountBalance = incomeSum - expenseSum;
            allCapitals.Add(reserveCapital);

            return allCapitals;
        }
        public List<LiabilityViewModel> GetLiabilityAccounts()
        {
            var liability = glactRepo.GetByMainCategory("Liability");

            var allLiabilityAccounts = new List<LiabilityViewModel>();

            foreach (var account in liability)
            {
                var model = new LiabilityViewModel();
                model.AccountName = account.Name;
                model.Amount = account.acountBalance;

                allLiabilityAccounts.Add(model);

            }
            //adding customer's savings and loan accounts since they are liabilities to the bank           
            var savingsAccounts = custRepo.GetByType("Savings");
            var savingsLiability = new LiabilityViewModel();
            savingsLiability.AccountName = "Savings Accounts";
            savingsLiability.Amount = savingsAccounts != null ? savingsAccounts.Sum(a => a.acctbalance) : 0;

            var currentAccounts = custRepo.GetByType("Current");
            var currentLiability = new LiabilityViewModel();
            currentLiability.AccountName = "Current Accounts";
            currentLiability.Amount = currentAccounts != null ? currentAccounts.Sum(a => a.acctbalance) : 0;

            allLiabilityAccounts.Add(savingsLiability);
            allLiabilityAccounts.Add(currentLiability);
            return allLiabilityAccounts;
        }
    }
}
