using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class GlPostingLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public string CheckdebitBalance(decimal balance, decimal Amount)
        {
            string status = "Successful";
            if (balance < Amount)
            {
                status = "Failed";
            }
            return status;
        }
        public string CheckcreditBalance(decimal balance, decimal Amount)
        {
            string status = "Successful";
            if (balance < Amount)
            {
                status = "Failed";
            }
            return status;
        }

        public decimal Debiting(string mainCategory,string glAccountName,decimal Amount)
        {
            decimal debitbalance = 0;
            var FindDebitAccount = _context.GlAccounts.Where(a => a.Name == glAccountName).FirstOrDefault();
            switch (mainCategory)
            {
                case "Asset":
                    debitbalance = FindDebitAccount.acountBalance + Amount;
                    return debitbalance;
                case "Expenses":
                    debitbalance = FindDebitAccount.acountBalance + Amount;
                    return debitbalance;
                case "Liability":
                    debitbalance = FindDebitAccount.acountBalance - Amount;
                    return debitbalance;
                case "Capital":
                    debitbalance = FindDebitAccount.acountBalance - Amount;
                    return debitbalance;
                case "Income":
                    debitbalance = FindDebitAccount.acountBalance - Amount;
                    return debitbalance;
            }
            return debitbalance;
        }

        public string CalculateDebitBalance(long GlAccountToDebitCode, decimal Amount, decimal creditacountBalance)
        {
            var FindDebitAccount = _context.GlAccounts.Where(a => a.Code == GlAccountToDebitCode).FirstOrDefault();
            decimal debitbalance = 0;
            string debitstatus = "";
            //determine balance for gl debit account
            if (FindDebitAccount.mainCategory == "Asset" || FindDebitAccount.mainCategory == "Expenses")
            {
                debitstatus = CheckcreditBalance(creditacountBalance, Amount);
                if (debitstatus == "Successful")
                {
                    debitbalance = FindDebitAccount.acountBalance + Amount;

                }
            }

            else if (FindDebitAccount.mainCategory == "Capital" || FindDebitAccount.mainCategory == "Liability" || FindDebitAccount.mainCategory == "Income")
            {
                debitstatus = CheckdebitBalance(FindDebitAccount.acountBalance, Amount);
                if (debitstatus == "Successful")
                {
                    debitbalance = FindDebitAccount.acountBalance - Amount;
                }

            }
            return debitstatus + "%" + debitbalance.ToString();

        }

        public string CalculateCreditBalance(long GlAccountToCreditCode, decimal Amount, decimal debitacountBalance)
        {
            var FindCreditAccount = _context.GlAccounts.Where(a => a.Code == GlAccountToCreditCode).FirstOrDefault();
            decimal creditbalance = 0;
            string creditstatus = "";
            //determine balance for gl debit account
            if (FindCreditAccount.mainCategory == "Asset" || FindCreditAccount.mainCategory == "Expenses")
            {
                creditstatus = CheckcreditBalance(FindCreditAccount.acountBalance, Amount);
                if (creditstatus == "Successful")
                {
                    creditbalance = FindCreditAccount.acountBalance - Amount;
                }
            }

            else if (FindCreditAccount.mainCategory == "Capital" || FindCreditAccount.mainCategory == "Liability" || FindCreditAccount.mainCategory == "Income")
            {
                creditstatus = CheckdebitBalance(debitacountBalance, Amount);
                if (creditstatus == "Successful")
                {
                    creditbalance = FindCreditAccount.acountBalance + Amount;
                }
            }

            return creditstatus + "%" + creditbalance.ToString();

        }

        public bool IsDebitable(GlAccount account, decimal Amount)
        {
            if(account.mainCategory=="Liability"|| account.mainCategory == "Income" || account.mainCategory == "Capital")
            {
                if (account.acountBalance >= Amount)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
        public bool IsCreditable(GlAccount account, decimal Amount)
        {
            if (account.mainCategory == "Asset" || account.mainCategory == "Expenses")
            {
                if (account.acountBalance >= Amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

    }
}
