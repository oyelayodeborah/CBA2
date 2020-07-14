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
    public class TellerPostingLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        //CustomerAccountRepository custAcctRepo = new CustomerAccountRepository(new ApplicationDbContext());
        //TellerRepository tellerRepo = new TellerRepository(new ApplicationDbContext());
        //GlAccountRepository glAcctRepo = new GlAccountRepository(new ApplicationDbContext());
        //TellerPostingLogic tellerPostingLogic = new TellerPostingLogic();
        //SavingsAccountRepository savAcctRepo = new SavingsAccountRepository(new ApplicationDbContext());
        //CurrentAccountRepository curAcctRepo = new CurrentAccountRepository(new ApplicationDbContext());

        Teller teller = new Teller();
        GlAccount glAccount = new GlAccount();
        GlAccount cotglAccount = new GlAccount();
        CustomerAccount customerAccount = new CustomerAccount();
        COTLog cotlog = new COTLog();




        ////Deposit
        //public void Deposit(decimal custacctbalance, decimal tillacctbalance, decimal glacctbalance, decimal Amount, long customerAcctNum, string getteller)
        //{

        //    decimal custbalance = 0;
        //    decimal glbalance = 0;
        //    decimal tellerbalance = 0;
        //    custbalance = custacctbalance + Amount;
        //    tellerbalance = tillacctbalance + Amount;
        //    glbalance = glacctbalance + Amount;
        //    PostDeposit(custbalance, tellerbalance, glbalance, customerAcctNum, getteller);
        //}
        //public void PostDeposit(decimal custbalance, decimal tellerbalance, decimal glbalance, long customerAcctNum, string getteller)
        //{
        //    var findCustomerAccountDetails = custAcctRepo.GetByAcctNum(customerAcctNum);
        //    var findTellerDetails = tellerRepo.GetByUser(getteller);
        //    var findGlAccountDetails = glAcctRepo.GetByName(findTellerDetails.tillAccount);

        //    teller = tellerRepo.Get(findTellerDetails.id);
        //    teller.tillAccountBalance = tellerbalance;

        //    glAccount = glAcctRepo.Get(findGlAccountDetails.id);
        //    glAccount.acountBalance = glbalance;

        //    customerAccount = custAcctRepo.Get(findCustomerAccountDetails.id);
        //    customerAccount.acctbalance = custbalance;

        //    glAcctRepo.Update(glAccount);
        //    custAcctRepo.Update(customerAccount);
        //    tellerRepo.Update(teller);


        //}


       // Withdrawal
        public decimal CalculateCOT(decimal amount, decimal COT)
        {
            var calCOT = (amount / 1000) * COT;
            return calCOT;
        }
        public decimal CalculateWithdrawableAmount(string accType, decimal COT, decimal savlien, decimal curlien, decimal amount)
        {
            decimal withdrawableamount = 0;
            if (accType == "Savings")
            {
                withdrawableamount = savlien + amount;
            }
            else
            {
                var calCOT = CalculateCOT(amount, COT);
                withdrawableamount = curlien + amount + calCOT;
            }
            return withdrawableamount;
        }
        public bool Withdrawable(decimal custacctbalance, decimal tillacctbalance, string accType, decimal COT, decimal savlien, decimal curlien, decimal amount)
        {
            decimal withdrawableAmount = CalculateWithdrawableAmount(accType, COT, curlien, savlien, amount);
            if (custacctbalance >= withdrawableAmount)
            {
                if (tillacctbalance >= amount)
                    return true;
            }
            return false;
        }
        //public void Withdrawal(string accType, decimal custacctbalance, decimal tillacctbalance, decimal glacctbalance, decimal cotglacctbalance, decimal COT, decimal savlien, decimal curlien, decimal amount, long customerAcctNum, string getteller)
        //{
        //    bool withdrawable = Withdrawable(custacctbalance, tillacctbalance, accType, COT, savlien, curlien, amount);
        //    decimal custbalance = 0;
        //    decimal glbalance = 0;
        //    decimal tellerbalance = 0;
        //    decimal cotbalance = 0;
        //    if (withdrawable == true && accType == "Savings")
        //    {
        //        custbalance = CalculateWithdrawableAmount(accType, COT, savlien, curlien, amount);
        //        tellerbalance = tillacctbalance - amount;
        //        glbalance = glacctbalance - amount;

        //    }
        //    else if (withdrawable == true && accType == "Current")
        //    {
        //        custbalance = CalculateWithdrawableAmount(accType, COT, savlien, curlien, amount);
        //        tellerbalance = tillacctbalance - amount;
        //        glbalance = glacctbalance - amount;
        //        cotbalance = cotglacctbalance + CalculateCOT(amount, COT);
        //    }

        //    //PostWithdrawal(accType, custbalance, tellerbalance, glbalance, cotbalance, customerAcctNum, getteller);
        //}
        //public void PostWithdrawal(string accType, decimal custbalance, decimal tellerbalance, decimal glbalance, decimal cotbalance, long customerAcctNum, string getteller)
        //{
        //    var findCustomerAccountDetails = custAcctRepo.GetByAcctNum(customerAcctNum);
        //    var findTellerDetails = tellerRepo.GetByUser(getteller);
        //    var findGlAccountDetails = glAcctRepo.GetByName(findTellerDetails.tillAccount);
        //    var findCurrentAccountDetails = curAcctRepo.GetByCurrentExpense("Current Interest Expense");
        //    var findGlAccountCOTDetails = glAcctRepo.GetByName(findCurrentAccountDetails.COTIncomeGL);


        //    teller = tellerRepo.Get(findTellerDetails.id);
        //    teller.tillAccountBalance = tellerbalance;

        //    glAccount = glAcctRepo.Get(findGlAccountDetails.id);
        //    glAccount.acountBalance = glbalance;

        //    customerAccount = custAcctRepo.Get(findCustomerAccountDetails.id);
        //    customerAccount.acctbalance = custbalance;
        //    glAcctRepo.Update(glAccount);
        //    custAcctRepo.Update(customerAccount);
        //    tellerRepo.Update(teller);
        //    if (accType == "Current")
        //    {
        //        cotglAccount = glAcctRepo.Get(findGlAccountCOTDetails.id);
        //        cotglAccount.acountBalance = cotbalance;

        //        cotlog.customerAcctNum = findCustomerAccountDetails.acctNumber;
        //        cotlog.customerName = findCustomerAccountDetails.acctName;
        //        cotlog.TransactionDate = DateTime.Now;
        //        cotlog.GlAccountToCredit = findGlAccountCOTDetails.Name;
        //        cotlog.GlAccountToCreditCode = findGlAccountCOTDetails.Code;

        //        glAcctRepo.Update(cotglAccount);
        //        _context.COTLogs.Add(cotlog);
        //        _context.SaveChanges();
        //    }




        //}


    }
}
