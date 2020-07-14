using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class BusinessLogic
    {
        GlCategory GlCategory = new GlCategory();
        FinancialReportLogic frLogic = new FinancialReportLogic();
        GlAccountRepository glRepo = new GlAccountRepository();
        CustomerAccountRepository custActRepo = new CustomerAccountRepository();
        public bool CreditGl(GlAccount account, decimal amount)
        {
            try
            {
                switch (account.mainCategory.ToString())
                {
                    case "Asset":
                        account.acountBalance -= amount;
                        break;
                    case "Capital":
                        account.acountBalance += amount;
                        break;
                    case "Expenses":
                        account.acountBalance -= amount;
                        break;
                    case "Income":
                        account.acountBalance += amount;
                        break;
                    case "Liability":
                        account.acountBalance += amount;
                        break;
                    default:
                        break;
                }//end switch
                frLogic.CreateTransaction(account, amount, "Credit");
                glRepo.Update(account);

                //frLogic.CreateTransaction(account, amount, TransactionType.Credit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//end creditGl

        public bool DebitGl(GlAccount account, decimal amount)
        {
            try
            {
                switch (account.mainCategory.ToString())
                {
                    case "Asset":
                        account.acountBalance += amount;
                        break;
                    case "Capital":
                        account.acountBalance -= amount;
                        break;
                    case "Expenses":
                        account.acountBalance += amount;
                        break;
                    case "Income":
                        account.acountBalance -= amount;
                        break;
                    case "Liability":
                        account.acountBalance -= amount;
                        break;
                    default:
                        break;
                }//end switch
                frLogic.CreateTransaction(account, amount, "Debit");
                glRepo.Update(account);

                //frLogic.CreateTransaction(account, amount, TransactionType.Debit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//end DebitGl

        public bool CreditCustomerAccount(CustomerAccount account, decimal amount)
        {
            try
            {
                if (account.accType == "Loan")
                {
                    account.acctbalance -= amount;       //Loan accounts are assets to the bank
                    frLogic.CreateTransaction(account, amount, "Credit");
                    custActRepo.Update(account);
                }
                else
                {
                    account.acctbalance += amount;       //Savings and current accounts are liabilities to the bank
                    frLogic.CreateTransaction(account, amount, "Credit");
                    custActRepo.Update(account);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DebitCustomerAccount(CustomerAccount account, decimal amount)
        {
            try
            {
                if (account.accType == "Loan")
                {
                    account.acctbalance += amount;
                    frLogic.CreateTransaction(account, amount, "Debit");
                    custActRepo.Update(account);
                }
                else
                {
                    account.acctbalance -= amount;
                    frLogic.CreateTransaction(account, amount, "Debit");
                    custActRepo.Update(account);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CreditLoanAccount(CustomerAccount account, decimal amount)
        {
            // CustomerAccount account = new CustomerAccount();
            LoanCustAcct loanaccount = new LoanCustAcct();

            try
            {
                if (loanaccount.ServicingAccountId == account.id)
                {
                    account.acctbalance -= amount;
                    frLogic.CreateTransaction(account, amount, "Credit");
                    custActRepo.Update(account);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DebitLoanAccount(CustomerAccount account, decimal amount)
        {
            // CustomerAccount account = new CustomerAccount();
            LoanCustAcct loanaccount = new LoanCustAcct();
            try
            {
                if (loanaccount.ServicingAccountId == account.id)
                {
                    account.acctbalance += amount;
                    frLogic.CreateTransaction(account, amount, "Debit");
                    custActRepo.Update(account);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreditTillAccount(Teller account, decimal amount)
        {
            try
            {
                account.tillAccountBalance -= amount;       //Till accounts are assets to the bank
                frLogic.CreateTransaction(account, amount, "Credit");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DebitTillAccount(Teller account, decimal amount)
        {
            
                try
                {
                    account.tillAccountBalance += amount;       //Till accounts are assets to the bank
                    frLogic.CreateTransaction(account, amount, "Debit");

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            
        }
    }
}
