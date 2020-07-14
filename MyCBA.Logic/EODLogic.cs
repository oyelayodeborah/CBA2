using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class EODLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        DateTime today;

        BusinessLogic busLogic = new BusinessLogic();
        AccountConfigurationLogic acctConfigLogic = new AccountConfigurationLogic();
        FinancialReportLogic frLogic = new FinancialReportLogic();
        
        AccountConfigurationRepository acctConfigRepo = new AccountConfigurationRepository();
        CustomerAccountRepository custActRepo = new CustomerAccountRepository();
        LoanCustAcctRepository loanRepo = new LoanCustAcctRepository();

        GlAccountRepository glRepo = new GlAccountRepository();

        //BusinessConfig config = new BusinessConfig();
        AccountConfiguration config = new AccountConfiguration();
        CurrentAcctMgt curAcctMgt = new CurrentAcctMgt();
        LoanAcctMgt loanAcctMgt = new LoanAcctMgt();
        SavingsAcctMgt savAcctMgt = new SavingsAcctMgt();
        GlAccount loanAcctGl = new GlAccount();
        CustomerAccount custLoanAcc = new CustomerAccount();
        GlAccount loanInterestIncomeGl = new GlAccount();
        GlAccount savInterestExpenseGl = new GlAccount();
        GlAccount curInterestExpenseGl = new GlAccount();
        GlAccount savInterestPayableGl = new GlAccount();
        GlAccount curInterestPayableGl = new GlAccount();





        int[] daysInMonth = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public EODLogic()
        {
            config = _context.AccountConfigurations.First();
            today = config.FinancialDate;
        }


        public void Open()
        {
            var appDbContext = new ApplicationDbContext();

            var GetbusinessConfig = appDbContext.BusinessConfigs.Single();
            BusinessConfig businessConfig = appDbContext.BusinessConfigs.Single();

            AccountConfiguration acountConfig = acctConfigRepo.Get(GetbusinessConfig.id);
            acountConfig.IsBusinessOpen = true;
            acctConfigRepo.Update(acountConfig);

            businessConfig.IsBusinessOpen = true;
            appDbContext.Entry(businessConfig).State = EntityState.Modified;
            appDbContext.SaveChanges();
        }

        public void Close()
        {
            var appDbContext = new ApplicationDbContext();

            var GetbusinessConfig = appDbContext.AccountConfigurations.Single();
            BusinessConfig businessConfig = appDbContext.BusinessConfigs.Single();

            AccountConfiguration acountConfig = acctConfigRepo.Get(GetbusinessConfig.id);
            acountConfig.IsBusinessOpen = false;
            acctConfigRepo.Update(acountConfig);

            businessConfig.IsBusinessOpen = false;
            appDbContext.Entry(businessConfig).State = EntityState.Modified;
            appDbContext.SaveChanges();
        }

        public string RunEOD()
        { string output = "";
            //check if all configurations are set
            try
            {
                if (IsConfigurationSet())
                {
                    SaveLoanInterestAccrued();

                    SaveCustomerInterestAccrued();
                    
                    SaveDailyIncomeAndExpenseBalance();     //to calculate Profit and Loss

                    config.FinancialDate = config.FinancialDate.AddDays(1);        //increments the financial date at the EOD
                    BusinessConfig busConfig = _context.BusinessConfigs.First();
                    busConfig.FinancialDate= config.FinancialDate;
                    _context.Entry(config).State = EntityState.Modified;
                    _context.SaveChanges();
                    _context.Entry(busConfig).State = EntityState.Modified;
                    _context.SaveChanges();
                    Close();

                    //Ensures all or none of the 5 steps above executes and gets saved                     
                    output = "Successfully Run EOD!";
                }
                else
                {
                    output = "Configurations not set!";
                }
            }
            catch (Exception)
            {
                output = "An error occured while running EOD";
            }
            return output;
        }


        

        public void SaveCustomerInterestAccrued()
        {
            CustomerAccountRepository custActRepo = new CustomerAccountRepository();
            int[] daysInMonth = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var financialDate = config.FinancialDate;
            var presentDay = financialDate.Day;
            var presentMonth = financialDate.Month;
            //Financial Date Has not been Increased!!!
            //var daysLeftInMonth = (daysInMonth[presentMonth - 1] - presentDay) + 1;

            var customerAccounts = custActRepo.GetAll();
            var savingsConfig = acctConfigRepo.GetAllSavingsConfig().Single();
            var currentConfig = acctConfigRepo.GetAllCurrentConfig().Single();

            foreach (var customerAccount in customerAccounts)
            {
                decimal monthlyInterestRemaining = 0;
                decimal interestPayable = 0;

                if (customerAccount.accType == "Current") //for current accounts
                {
                    CustomerAccount custAcct = custActRepo.Get(customerAccount.id);
                    curInterestExpenseGl = glRepo.Get(currentConfig.CurrentInterestExpenseGLId);
                    curInterestPayableGl = glRepo.Get(currentConfig.CurrentInterestPayableGLId);

                    if (customerAccount.acctbalance >= currentConfig.CurrentMinimumBalance)//check if customer is eligible for receiving interest
                    {
                        monthlyInterestRemaining = customerAccount.acctbalance * (currentConfig.CurrentCreditInterestRate / 100) *
                                               (daysInMonth[presentMonth - 1]);
                        interestPayable = monthlyInterestRemaining / daysInMonth[presentMonth - 1];
                        //increment customer Interest accrued and disburse if its month end
                        //customerAccount.dailyInterestAccrued += interestPayable;
                        custAcct.dailyInterestAccrued += interestPayable;

                        //credit the interest payable gl for record purpose and debit interest expense account
                        busLogic.DebitGl(currentConfig.CurrentInterestExpenseGL, interestPayable);
                        busLogic.CreditGl(currentConfig.CurrentInterestPayableGL, interestPayable);
                        //log transaction...
                        //frLogic.CreateTransaction(currentConfig.CurrentInterestExpenseGL, interestPayable, "Debit");
                        //frLogic.CreateTransaction(currentConfig.CurrentInterestPayableGL, interestPayable, "Credit");
                        //curInterestExpenseGl = glRepo.Get(currentConfig.CurrentInterestExpenseGLId);
                        //curInterestExpenseGl.acountBalance += interestPayable;
                        //curInterestPayableGl = glRepo.Get(currentConfig.CurrentInterestPayableGLId);
                        //curInterestPayableGl.acountBalance += interestPayable;

                        custActRepo.Update(custAcct);
                       
                    }
                }
                else
                {
                    CustomerAccount custAcct = custActRepo.Get(customerAccount.id);
                    savInterestExpenseGl = glRepo.Get(savingsConfig.SavingsInterestExpenseGLId);
                    savInterestPayableGl = glRepo.Get(savingsConfig.SavingsInterestPayableGLId);

                    if (customerAccount.acctbalance >= savingsConfig.SavingsMinimumBalance)
                    {
                        //for Savings account
                        monthlyInterestRemaining = customerAccount.acctbalance * (savingsConfig.SavingsCreditInterestRate / 100) *
                                               (daysInMonth[presentMonth - 1]);
                        interestPayable = monthlyInterestRemaining / daysInMonth[presentMonth - 1];
                        //increment customer Interest accrued and disburse if its month end
                        //customerAccount.dailyInterestAccrued += interestPayable;
                        custAcct.dailyInterestAccrued += interestPayable;
                        //credit the interest payable gl for record purpose and debit interest expense account
                        busLogic.DebitGl(savingsConfig.SavingsInterestExpenseGL, interestPayable);
                        busLogic.CreditGl(savingsConfig.SavingsInterestPayableGL, interestPayable);

                        //log transaction...

                        //frLogic.CreateTransaction(savingsConfig.SavingsInterestExpenseGL, interestPayable, "Debit");
                        //frLogic.CreateTransaction(savingsConfig.SavingsInterestPayableGL, interestPayable, "Credit");
                        //savInterestExpenseGl = glRepo.Get(savingsConfig.SavingsInterestExpenseGLId);
                        //savInterestPayableGl = glRepo.Get(savingsConfig.SavingsInterestPayableGLId);
                        //savInterestPayableGl.acountBalance += interestPayable;
                        //savInterestExpenseGl.acountBalance += interestPayable;

                        custActRepo.Update(custAcct);
                        
                    }
                    //custActRepo.Update(customerAccount);

                }

            }
            var value = daysInMonth[presentMonth - 1];
            var day = presentDay;
            if (presentDay == daysInMonth[presentMonth - 1])
            {
                foreach (var customerAccount in customerAccounts)
                {
                    CustomerAccount custAcct = custActRepo.Get(customerAccount.id);
                    var amount = custAcct.dailyInterestAccrued;
                    var bal = amount;
                    busLogic.CreditCustomerAccount(custAcct, amount);
                    busLogic.DebitGl(
                        custAcct.accType == "Current"
                            ? currentConfig.CurrentInterestPayableGL
                            : savingsConfig.SavingsInterestPayableGL, amount);
                    custAcct.dailyInterestAccrued = 0;

                    custActRepo.Update(custAcct);
                    //busLogic.CreditGl(
                    //    customerAccount.accType == "Current"
                    //        ? currentConfig.CurrentInterestExpenseGL
                    //        : savingsConfig.SavingsInterestExpenseGL, customerAccount.dailyInterestAccrued);
                    //log transaction ...
                    //frLogic.CreateTransaction(customerAccount, customerAccount.dailyInterestAccrued, "Debit");
                    //frLogic.CreateTransaction(customerAccount.accType == "Current"
                    //        ? currentConfig.CurrentInterestPayableGL
                    //        : savingsConfig.SavingsInterestPayableGL, customerAccount.dailyInterestAccrued, "Credit");
                    //reset customers interest accrued;
                    //customerAccount.dailyInterestAccrued = 0;

                    //if (customerAccount.accType == "Savings")
                    //{
                    //    //savInterestExpenseGl = glRepo.Get(savingsConfig.SavingsInterestExpenseGLId);
                    //    //savInterestExpenseGl.acountBalance = 0;
                    //    //savInterestPayableGl = glRepo.Get(savingsConfig.SavingsInterestPayableGLId);
                    //    //savInterestPayableGl.acountBalance =0;

                    //    busLogic.DebitGl(savInterestExpenseGl, 0);
                    //    busLogic.CreditGl(savInterestPayableGl, 0);

                    //    glRepo.Update(savInterestExpenseGl);
                    //    glRepo.Update(savInterestPayableGl);
                    //}
                    //else
                    //{
                    //    //curInterestExpenseGl = glRepo.Get(currentConfig.CurrentInterestExpenseGLId);
                    //    //curInterestExpenseGl.acountBalance =0;
                    //    //curInterestPayableGl = glRepo.Get(currentConfig.CurrentInterestPayableGLId);
                    //    //curInterestPayableGl.acountBalance =0;

                    //    busLogic.DebitGl(curInterestExpenseGl, 0);
                    //    busLogic.CreditGl(curInterestPayableGl, 0);

                    //    glRepo.Update(curInterestExpenseGl);
                    //    glRepo.Update(curInterestPayableGl);
                    //}


                }
            }
        }

        public void SaveLoanInterestAccrued()
        {
            var loanConfig = acctConfigRepo.GetAllLoanConfig().Single();
            var loanAcct = loanRepo.GetAllUnPaid();
            var loanAccountGl = glRepo.GetLoanAccount();
            loanAcctGl = glRepo.Get(loanAccountGl.id);

            if (loanAcct != null)
            {

                var interestIncomeGlAccount = glRepo.Get(loanConfig.LoanInterestIncomeGLId);


                foreach (var account in loanAcct)
                {
                    //var loanAccountId = _customerAccount.GetByAccountNumber(account.AccountNumber);
                    var customerAccount = custActRepo.Get(account.ServicingAccountId);//_customerAccount.Get(account.CustomerAccountId);

                    if (account.DurationInMonths * 30 != account.DaysCount)
                    {
                        account.LoanAmountRemaining -= account.LoanAmountReduction;
                        account.LoanInterestRemaining -= account.LoanInterestReduction;


                        //payment of interest
                        busLogic.CreditGl(interestIncomeGlAccount, account.LoanInterestReduction);
                        busLogic.DebitCustomerAccount(customerAccount, account.LoanInterestReduction);




                        //paying daily loan back
                        busLogic.DebitCustomerAccount(customerAccount, account.LoanAmountReduction);
                        busLogic.CreditGl(loanAcctGl, account.LoanAmountReduction);



                        loanRepo.Update(account);
                    }

                    if (account.DurationInMonths * 30 == account.DaysCount)
                    {
                        account.status = "Paid";
                        account.DaysCount = 0;
                        loanRepo.Update(account);

                    }

                    //increases days if loan payment not reached
                    else if (account.DurationInMonths * 30 != account.DaysCount)
                    {
                        account.DaysCount++;
                        loanRepo.Update(account);

                    }
                }
            }
        }



        public void SaveDailyIncomeAndExpenseBalance()
        {
            var allIncomes = glRepo.GetByMainCategory("Income");
            //save daily balance of all income acccounts
            foreach (var account in allIncomes)
            {
                var entry = new ExpensesIncomeEntry();
                entry.AccountName = account.Name;
                entry.Amount = account.acountBalance;
                entry.Date = today;
                entry.EntryType = ExpensesIncomeEntry.PandLType.Income;
                _context.ExpensesIncomeEntries.Add(entry);
                _context.SaveChanges();
                //new ProfitAndLossRepository().Insert(entry);
            }

            //save daily balance off all expense accounts
            var allExpenses = glRepo.GetByMainCategory("Expenses");
            foreach (var account in allExpenses)
            {
                var entry = new ExpensesIncomeEntry();
                entry.AccountName = account.Name;
                entry.Amount = account.acountBalance;
                entry.Date = today;
                entry.EntryType = ExpensesIncomeEntry.PandLType.Expenses;
                _context.ExpensesIncomeEntries.Add(entry);
                _context.SaveChanges();
                //new ProfitAndLossRepository().Insert(entry);
            }
        }



        public bool IsConfigurationSet()
        {
            var getSavingsConfig = acctConfigRepo.GetAllSavingsConfig().Single();

            var getCurrentConfig = acctConfigRepo.GetAllCurrentConfig().Single();

            var getLoanConfig = acctConfigRepo.GetAllLoanConfig().Single();

            if (getSavingsConfig.SavingsInterestExpenseGLId == 0 || getSavingsConfig.SavingsInterestPayableGLId == 0 || getCurrentConfig.CurrentInterestExpenseGLId == 0
                || getCurrentConfig.CurrentCotIncomeGLId == 0 || getLoanConfig.LoanInterestExpenseGLId == 0 || getLoanConfig.LoanInterestIncomeGLId == 0 ||
                 getSavingsConfig.SavingsInterestExpenseGLId == null || getSavingsConfig.SavingsInterestPayableGLId == null
                || getCurrentConfig.CurrentInterestExpenseGLId ==null || getCurrentConfig.CurrentCotIncomeGLId == null /*|| getLoanConfig.LoanInterestExpenseGLId == null*/ 
                || getLoanConfig.LoanInterestIncomeGLId == null || getCurrentConfig.CurrentInterestPayableGLId==null
                || getCurrentConfig.CurrentInterestPayableGLId==0)
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
