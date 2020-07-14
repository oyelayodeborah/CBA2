using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class FinancialReportLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public void CreateTransaction(GlAccount account, decimal amount, string trnType)
        {
            GlCategoryRepository glCatRepo = new GlCategoryRepository();
            var getGlCat = glCatRepo.Get(account.GlCategoryId);
            //Record this transaction for Trial Balance generation
            Transaction transaction = new Transaction();
            transaction.Amount = amount;
            AccountConfiguration accountConfig = _context.AccountConfigurations.ToList().Single();
            if (accountConfig != null)
            {
                transaction.Date = accountConfig.FinancialDate;
            }
            else
            {
                transaction.Date = DateTime.Now;
            }
            transaction.AccountName = account.Name;
            transaction.SubCategory = getGlCat.name;
            transaction.MainCategory = getGlCat.mainAccountCategory.ToString();
            transaction.TransactionType = trnType;

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void CreateTransaction(CustomerAccount account, decimal amount, string trnType)
        {
            if (account.accType == "Loan")
            {

                //Record this transaction for Trial Balance generation
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                AccountConfiguration accountConfig = _context.AccountConfigurations.ToList().Single();
                if (accountConfig != null)
                {
                    transaction.Date = accountConfig.FinancialDate;
                }
                else
                {
                    transaction.Date = DateTime.Now;
                }
                transaction.AccountName = account.acctName;
                transaction.SubCategory = "Customer's Loan Account";
                transaction.MainCategory = "Asset";
                transaction.TransactionType = trnType;

                _context.Transactions.Add(transaction);
                _context.SaveChanges();
            }
            else
            {
                //Record this transaction for Trial Balance generation
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                AccountConfiguration accountConfig = _context.AccountConfigurations.ToList().Single();
                if (accountConfig != null)
                {
                    transaction.Date = accountConfig.FinancialDate;
                }
                else
                {
                    transaction.Date = DateTime.Now;
                }
                transaction.AccountName = account.acctName;
                transaction.SubCategory = "Customer Account";
                transaction.MainCategory = "Liability";
                transaction.TransactionType = trnType;

                _context.Transactions.Add(transaction);
                _context.SaveChanges();
            }
        }
        public void CreateTransaction(Teller account, decimal amount, string trnType)
        {
            GlAccountRepository glAccRepo = new GlAccountRepository();
            GlAccount glAccount = glAccRepo.Get(account.TillAccountId);
            GlCategoryRepository glCatRepo = new GlCategoryRepository();
            GlCategory glCategory = glCatRepo.Get(glAccount.GlCategoryId);
                //Record this transaction for Trial Balance generation
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
            AccountConfiguration accountConfig = _context.AccountConfigurations.ToList().Single();
            if (accountConfig != null)
            {
                transaction.Date = accountConfig.FinancialDate;
            }
            else
            {
                transaction.Date = DateTime.Now;
            }
            transaction.AccountName = glAccount.Name;
                transaction.SubCategory = glCategory.name;
                transaction.MainCategory = "Asset";
                transaction.TransactionType = trnType;

                _context.Transactions.Add(transaction);
                _context.SaveChanges();
            }
           
        }



    }

