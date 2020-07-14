using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class AccountConfigurationLogic
    {
        AccountConfigurationRepository acctConfigRepo = new AccountConfigurationRepository();

        //AccountConfiguration accountConfig = new AccountConfiguration();
        public void SaveSavingsConfig(SavingsAcctMgt savingsAcctMgt)
        {
            AccountConfiguration accountConfig = new AccountConfiguration();

            accountConfig.SavingsCreditInterestRate = savingsAcctMgt.SavingsCreditInterestRate;
            accountConfig.SavingsMinimumBalance = savingsAcctMgt.SavingsMinimumBalance;
            accountConfig.SavingsInterestExpenseGLId = savingsAcctMgt.SavingsInterestExpenseGLId;
            accountConfig.SavingsInterestPayableGLId = savingsAcctMgt.SavingsInterestPayableGLId;
            var getAcctConfig = acctConfigRepo.GetAll();
            if (getAcctConfig.Count() == 0)
            {
                var CheckStatus = checkStatus();
                if (CheckStatus == false)
                {
                    accountConfig.status = "Incomplete";
                }
                else
                {
                    accountConfig.status = "Complete";

                }
                acctConfigRepo.Save(accountConfig);
            }
            else
            {
                var config = getAcctConfig.Single();
                accountConfig = acctConfigRepo.Get(config.id);
                accountConfig.SavingsCreditInterestRate = savingsAcctMgt.SavingsCreditInterestRate;
                accountConfig.SavingsMinimumBalance = savingsAcctMgt.SavingsMinimumBalance;
                accountConfig.SavingsInterestExpenseGLId = savingsAcctMgt.SavingsInterestExpenseGLId;
                accountConfig.SavingsInterestPayableGLId = savingsAcctMgt.SavingsInterestPayableGLId;
                var CheckStatus = checkStatus();
                if (CheckStatus == false)
                {
                    accountConfig.status = "Incomplete";
                }
                else
                {
                    accountConfig.status = "Complete";

                }
                acctConfigRepo.Update(accountConfig);
            }
        }

        public void SaveLoanConfig(LoanAcctMgt loanAcctMgt)
        {
            AccountConfiguration accountConfig = new AccountConfiguration();

            accountConfig.LoanDebitInterestRate = loanAcctMgt.LoanDebitInterestRate;
            accountConfig.LoanInterestExpenseGLId = loanAcctMgt.LoanInterestExpenseGLId;
            accountConfig.LoanInterestIncomeGLId = loanAcctMgt.LoanInterestIncomeGLId;
            accountConfig.LoanInterestReceivableGLId = loanAcctMgt.LoanInterestReceivableGLId;
            var getAcctConfig = acctConfigRepo.GetAll();
            if (getAcctConfig.Count() == 0)
            {
                var CheckStatus = checkStatus();
                if (CheckStatus == false)
                {
                    accountConfig.status = "Incomplete";
                }
                else
                {
                    accountConfig.status = "Complete";

                }
                acctConfigRepo.Save(accountConfig);
            }
            else
            {
                var config = getAcctConfig.Single();
                accountConfig = acctConfigRepo.Get(config.id);
                accountConfig.LoanDebitInterestRate = loanAcctMgt.LoanDebitInterestRate;
                accountConfig.LoanInterestExpenseGLId = loanAcctMgt.LoanInterestExpenseGLId;
                accountConfig.LoanInterestIncomeGLId = loanAcctMgt.LoanInterestIncomeGLId;
                accountConfig.LoanInterestReceivableGLId = loanAcctMgt.LoanInterestReceivableGLId;
                var CheckStatus = checkStatus();
                if (CheckStatus == false)
                {
                    accountConfig.status = "Incomplete";
                }
                else
                {
                    accountConfig.status = "Complete";

                }
                acctConfigRepo.Update(accountConfig);
            }
        }

        public void SaveCurrentConfig(CurrentAcctMgt currentAcctMgt)
        {
            AccountConfiguration accountConfig = new AccountConfiguration();

            accountConfig.CurrentCot = currentAcctMgt.CurrentCot;
            accountConfig.CurrentCotIncomeGLId = currentAcctMgt.CurrentCotIncomeGLId;
            accountConfig.CurrentCreditInterestRate = currentAcctMgt.CurrentCreditInterestRate;
            accountConfig.CurrentInterestExpenseGLId = currentAcctMgt.CurrentInterestExpenseGLId;
            accountConfig.CurrentInterestPayableGLId = currentAcctMgt.CurrentInterestPayableGLId;
            accountConfig.CurrentMinimumBalance = currentAcctMgt.CurrentMinimumBalance;
            var getAcctConfig = acctConfigRepo.GetAll();
            if (getAcctConfig.Count() == 0)
            {
                var CheckStatus = checkStatus();
                if (CheckStatus == false)
                {
                    accountConfig.status = "Incomplete";
                }
                else
                {
                    accountConfig.status = "Complete";

                }
                acctConfigRepo.Save(accountConfig);
            }
            else
            {
                var config = getAcctConfig.Single();
                accountConfig = acctConfigRepo.Get(config.id);
                accountConfig.CurrentCot = currentAcctMgt.CurrentCot;
                accountConfig.CurrentCotIncomeGLId = currentAcctMgt.CurrentCotIncomeGLId;
                accountConfig.CurrentCreditInterestRate = currentAcctMgt.CurrentCreditInterestRate;
                accountConfig.CurrentInterestExpenseGLId = currentAcctMgt.CurrentInterestExpenseGLId;
                accountConfig.CurrentInterestPayableGLId = currentAcctMgt.CurrentInterestPayableGLId;
                accountConfig.CurrentMinimumBalance = currentAcctMgt.CurrentMinimumBalance;
                var CheckStatus = checkStatus();
                if (CheckStatus == false)
                {
                    accountConfig.status = "Incomplete";
                }
                else
                {
                    accountConfig.status = "Complete";

                }
                acctConfigRepo.Update(accountConfig);
            }

        }

        public void SaveBusinessConfig(BusinessConfig businessConfig)
        {
            AccountConfiguration accountConfig = new AccountConfiguration();


            accountConfig.FinancialDate= businessConfig.FinancialDate;
            accountConfig.IsBusinessOpen = businessConfig.IsBusinessOpen;
            accountConfig.status = "Incomplete";
            var getAcctConfig= acctConfigRepo.GetAll();
            if (getAcctConfig.Count() == 0)
            {
                acctConfigRepo.Save(accountConfig);
            }
            else
            {
                var config = getAcctConfig.Single();
                accountConfig = acctConfigRepo.Get(config.id);
                accountConfig.FinancialDate = businessConfig.FinancialDate;
                accountConfig.IsBusinessOpen = businessConfig.IsBusinessOpen;
                acctConfigRepo.Update(accountConfig);
            }
        }

        public bool checkStatus()
        {
            var getSavingsConfig = new SavingsAcctMgt();

            var getCurrentConfig = new CurrentAcctMgt();

            var getLoanConfig = new LoanAcctMgt();
            if (!(getSavingsConfig.SavingsInterestExpenseGLId == 0 || getSavingsConfig.SavingsInterestPayableGLId == 0 || getCurrentConfig.CurrentInterestExpenseGLId == 0
                || getCurrentConfig.CurrentCotIncomeGLId == 0 || getLoanConfig.LoanInterestExpenseGLId == 0 || getLoanConfig.LoanInterestIncomeGLId == 0 ||
                 getSavingsConfig.SavingsInterestExpenseGLId == null || getSavingsConfig.SavingsInterestPayableGLId == null
                || getCurrentConfig.CurrentInterestExpenseGLId == null || getCurrentConfig.CurrentCotIncomeGLId == null /*|| getLoanConfig.LoanInterestExpenseGLId == null*/
                || getLoanConfig.LoanInterestIncomeGLId == null || getCurrentConfig.CurrentInterestPayableGLId == null
                || getCurrentConfig.CurrentInterestPayableGLId == 0))
            {
                getCurrentConfig=acctConfigRepo.GetAllCurrentConfig().Single();
                getSavingsConfig = acctConfigRepo.GetAllSavingsConfig().Single();
                getLoanConfig = acctConfigRepo.GetAllLoanConfig().Single();
            }
            


            if (getSavingsConfig.SavingsInterestExpenseGLId == 0 || getSavingsConfig.SavingsInterestPayableGLId == 0 || getCurrentConfig.CurrentInterestExpenseGLId == 0
                || getCurrentConfig.CurrentCotIncomeGLId == 0 || getLoanConfig.LoanInterestExpenseGLId == 0 || getLoanConfig.LoanInterestIncomeGLId == 0 ||
                 getSavingsConfig.SavingsInterestExpenseGLId == null || getSavingsConfig.SavingsInterestPayableGLId == null
                || getCurrentConfig.CurrentInterestExpenseGLId == null || getCurrentConfig.CurrentCotIncomeGLId == null /*|| getLoanConfig.LoanInterestExpenseGLId == null*/
                || getLoanConfig.LoanInterestIncomeGLId == null || getCurrentConfig.CurrentInterestPayableGLId == null
                || getCurrentConfig.CurrentInterestPayableGLId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
