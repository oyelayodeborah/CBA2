using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class CustomerAccountLogic
    {
        public Random _random = new Random();
        ApplicationDbContext _context = new ApplicationDbContext();
        CustomerAccountRepository custAccRepo = new CustomerAccountRepository();
        BaseRepository<AccountConfiguration> configRepo = new BaseRepository<AccountConfiguration>(new ApplicationDbContext());



        public long GenerateAccountNumber(string accType, int customer)
        {
            long acctNum = 0;
            var value= _random.Next(0, 999).ToString("D3");
            var findDetails = _context.Customers.Where(a => a.id == customer).FirstOrDefault();
            if (accType == "Savings")
            {
                string id = Convert.ToString(findDetails.customerID);
                var acctNumber = "1" + value + id;
                acctNum = Convert.ToInt64(acctNumber);
            }
            else if(accType == "Current")
             {
                string id = Convert.ToString(findDetails.customerID);
                var acctNumber = "2" + value + id;
                acctNum = Convert.ToInt64(acctNumber);
            }
            else if(accType=="Loan")
            {
                string id = Convert.ToString(findDetails.customerID);
                var acctNumber = "3" + value + id;
                acctNum = Convert.ToInt64(acctNumber);
            }
            var checklength = Convert.ToString(acctNum);
            var count = checklength.Count();
            if (count > 10)
            {
                for (int i = 11; i <= count; )
                {
                    int startval = 0;
                    int endval = 1;
                    int lastIndex = checklength.IndexOf("0", startval);
                    int? getval = lastIndex;
                    if (getval != null)
                    {
                        string removeZero = checklength.Remove(lastIndex, endval);
                        acctNum = Convert.ToInt64(removeZero);
                    }
                    else
                    {
                        string removeZero = checklength.Remove(1,2);
                        acctNum= Convert.ToInt64(removeZero);
                    }
                    i++;
                    return acctNum;
                }
            }
            
            return acctNum;
        }
        public bool IsDetailsExist(string GlAccountName)
        {
            var findDetails = _context.GlAccounts.Where(a => a.Name == GlAccountName).FirstOrDefault();

            if (findDetails == null)
            {
                return false;
            }
            return true;
        }
        public void Closed(int? id)
        {
            CustomerAccount customerAccount = custAccRepo.Get(id);
            customerAccount.status = "Closed";
            try
            {

                //Adding customerAccount info to memory
                _context.Entry(customerAccount).State = EntityState.Modified;

                //Updating customerAccount info to the database
                _context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);


            }

            // return RedirectToAction("Index", "CustomerAccounts");
        }
        public void Opened(int? id)
        {
            CustomerAccount customerAccount = custAccRepo.Get(id);
            customerAccount.status = "Opened";
            try
            {

                //Adding customerAccount info to memory
                _context.Entry(customerAccount).State = EntityState.Modified;

                //Updating customerAccount info to the database
                _context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);


            }

            // return RedirectToAction("Index", "CustomerAccounts");
        }

        public bool CustomerAccountHasSufficientBalance(CustomerAccount account, decimal amountToDebit)
        {
            var config = configRepo.GetAll().First();
            if (account.acctbalance >= amountToDebit + config.SavingsMinimumBalance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
