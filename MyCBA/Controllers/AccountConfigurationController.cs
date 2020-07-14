using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;
using MyCBA.ViewModels;

namespace MyCBA.Controllers
{
    [AdminRoleRestrictLogic]
    public class AccountConfigurationController : Controller
    {
        
        //BaseRepository<AccountConfiguration> baserepo = new BaseRepository<AccountConfiguration>(new ApplicationDbContext());
        AccountConfigurationRepository accountConfig = new AccountConfigurationRepository();
        AccountConfigurationLogic acctConfigLogic = new AccountConfigurationLogic();

        ApplicationDbContext _context = new ApplicationDbContext();
        //GlCategoryRepository glCatRepo = new GlCategoryRepository();
        GlAccountRepository glCatRepo = new GlAccountRepository();


        public ActionResult Index()
        {
            //var customers = baserepo.GetAll();
            //return View(customers.ToList());
            return View(_context.AccountConfigurations.ToList());
        }

        //------------------------------Savings-----------------------------------------------------------

        // GET: AccountConfiguration/Savings
        public ActionResult Savings()
        {
            var getSavingsAcctMgtTable = accountConfig.GetAllSavingsConfig();
            if (getSavingsAcctMgtTable.Count()>0 && getSavingsAcctMgtTable.Count()==1)
            {
                return RedirectToAction("UpdateSavings", "AccountConfiguration");
            }
            else
            {
                var model = new SavingsAcctMgt();
                var getExpense = glCatRepo.GetName("Savings Interest Expense");
                var getPayable = glCatRepo.GetName("Savings Interest Payable");
                if (getExpense != null)
                {
                    model.SavingsInterestExpense = getExpense;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                   c.GlCategoryId == getExpense.id);*/

                }
                else
                {
                    model.SavingsInterestExpense = null;
                }
                if (getPayable != null)
                {
                    model.SavingsInterestPayable = getPayable;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Liability" &&
                     c.GlCategoryId == getPayable.id);*/
                }
                else
                {
                    model.SavingsInterestPayable = null;
                }
                //var model = new SavingsAcctMgt()
                //{
                    
                //    SavingsInterestExpense = _context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" && 
                //    c.GlCategoryId == getExpense.id ),
                //    SavingsInterestPayable = _context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Liability" &&
                //     c.GlCategoryId == getPayable.id)
                //};
                return View(model);
            }
        }

        //POST: AccountConfiguration/Savings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Savings(SavingsAcctMgt savingsAcctMgt)
        {

            var getExpense = glCatRepo.GetName("Savings Interest Expense");
            var getPayable = glCatRepo.GetName("Savings Interest Payable");

            savingsAcctMgt.SavingsInterestExpense = getExpense; /*_context.GlAccounts.ToList().Where(c => /*c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                   c.GlCategoryId == getExpense.id);*/
            savingsAcctMgt.SavingsInterestPayable = getPayable; /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Liability" &&
             c.GlCategoryId == getPayable.id);*/

            if (ModelState.IsValid)
            {
                try
                {
                    var value = Convert.ToDecimal(0.00);
                    if (savingsAcctMgt.SavingsCreditInterestRate == value || savingsAcctMgt.SavingsCreditInterestRate == 0|| savingsAcctMgt.SavingsMinimumBalance == value || savingsAcctMgt.SavingsMinimumBalance == 0
                        || savingsAcctMgt.SavingsInterestExpenseGLId == null || savingsAcctMgt.SavingsInterestPayableGLId == null)
                    {
                        savingsAcctMgt.status = "Incomplete";
                    }
                    else
                    {
                        savingsAcctMgt.status = "Complete";
                    }
                    
                    //Adding savingsAcctMgt info to memory
                    _context.SavingsAcctMgts.Add(savingsAcctMgt);

                    //Saving savingsAcctMgt info to the database
                    _context.SaveChanges();

                    acctConfigLogic.SaveSavingsConfig(savingsAcctMgt);

                    ViewBag.Message = "Success";
                    return RedirectToAction("UpdateSavings", "AccountConfiguration");
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    ViewBag.Message = "Error";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }

                return View();
        }

        // GET: AccountConfiguration/UpdateSavings
        public ActionResult UpdateSavings()
        {
            var getSavingsAcctMgtTable = accountConfig.GetAllSavingsConfig();
            if (getSavingsAcctMgtTable.Count() == 0)
            {
                return RedirectToAction("Savings", "AccountConfiguration");
            }
            else {
                SavingsAcctMgt model = accountConfig.GetAllSavingsConfig().Single();
                if (model == null)
                {
                    return HttpNotFound();
                }

                var getExpense = glCatRepo.GetName("Savings Interest Expense");
                var getPayable = glCatRepo.GetName("Savings Interest Payable");

                model.SavingsInterestExpense = getExpense; /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                c.GlCategoryId == getExpense.id);*/
                model.SavingsInterestPayable = getPayable;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Liability" &&
                 c.GlCategoryId == getPayable.id);*/
                
                return View(model);
            }
        }

        //POST: AccountConfiguration/UpdateSavings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSavings([Bind(Include = "id,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGLId,SavingsInterestPayableGLId")] SavingsAcctMgt savingsAcctMgt)
        {

            var getExpense = glCatRepo.GetName("Savings Interest Expense");
            var getPayable = glCatRepo.GetName("Savings Interest Payable");

            savingsAcctMgt.SavingsInterestExpense = getExpense;/* _context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
            c.GlCategoryId == getExpense.id);*/
            savingsAcctMgt.SavingsInterestPayable = getPayable;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Liability" &&
             c.GlCategoryId == getPayable.id);*/

            if (ModelState.IsValid)
            {

                try
                {
                    var value = Convert.ToDecimal(0.00);
                    if (savingsAcctMgt.SavingsCreditInterestRate == value || savingsAcctMgt.SavingsCreditInterestRate == 0 || savingsAcctMgt.SavingsMinimumBalance == value || savingsAcctMgt.SavingsMinimumBalance == 0
                         || savingsAcctMgt.SavingsInterestExpenseGLId == null || savingsAcctMgt.SavingsInterestPayableGLId == null)
                    {
                        savingsAcctMgt.status = "Incomplete";
                    }
                    else
                    {
                        savingsAcctMgt.status = "Complete";
                    }
                    
                    //Adding savingsAcctMgt info to memory
                    _context.Entry(savingsAcctMgt).State = EntityState.Modified;

                    //Saving savingsAcctMgt info to the database
                    _context.SaveChanges();
                    acctConfigLogic.SaveSavingsConfig(savingsAcctMgt);

                    ViewBag.Message = "Success";
                    return RedirectToAction("UpdateSavings", "AccountConfiguration");
                }



                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    ViewBag.Message = "Error";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
            return View();
        }

        //------------------------------Current-----------------------------------------------------------

        // GET: AccountConfiguration/Current
        public ActionResult Current()
        {
            var getCurrentAcctMgtTable = accountConfig.GetAllCurrentConfig();

            if (getCurrentAcctMgtTable.Count() > 0 && getCurrentAcctMgtTable.Count() == 1)
            {
                return RedirectToAction("UpdateCurrent", "AccountConfiguration");
            }
            else
            {
                var getExpense = glCatRepo.GetName("Current Interest Expense");
                var getCotIncome = glCatRepo.GetName("Current Cot Income");
                var getPayable = glCatRepo.GetName("Current Interest Payable");


                var model = new CurrentAcctMgt()
                {
                    CurrentInterestExpense = getExpense,/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                    c.GlCategoryId == getExpense.id),*/
                    CurrentCotIncome =getCotIncome, /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
                     c.GlCategoryId == getCotIncome.id)*/
                    CurrentInterestPayable = getPayable
                };
                return View(model);
            }
        }

        //POST: AccountConfiguration/Current
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Current(CurrentAcctMgt currentAcctMgt)
        {
            var getExpense = glCatRepo.GetName("Current Interest Expense");
            var getCotIncome = glCatRepo.GetName("Current Cot Income");
            var getPayable= glCatRepo.GetName("Current Interest Payable");

            currentAcctMgt.CurrentInterestExpense = getExpense;/* _context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                    c.GlCategoryId == getExpense.id);*/
            currentAcctMgt.CurrentCotIncome = getCotIncome;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
             c.GlCategoryId == getCotIncome.id);*/
            currentAcctMgt.CurrentInterestPayable = getPayable;
            if (ModelState.IsValid)
            {
                try
                {
                    var value = Convert.ToDecimal(0.00);
                    if (currentAcctMgt.CurrentCreditInterestRate == value || currentAcctMgt.CurrentCreditInterestRate == 0 || currentAcctMgt.CurrentMinimumBalance == value || currentAcctMgt.CurrentMinimumBalance == 0
                        || currentAcctMgt.CurrentInterestPayableGLId == null || currentAcctMgt.CurrentInterestExpenseGLId == null || currentAcctMgt.CurrentCotIncomeGLId == null||currentAcctMgt.CurrentCot == value || currentAcctMgt.CurrentCot == 0 )
                    {
                        currentAcctMgt.status = "Incomplete";
                    }
                    else
                    {
                        currentAcctMgt.status = "Complete";
                    }

                    //Adding currentAcctMgt info to memory
                    _context.CurrentAcctMgts.Add(currentAcctMgt);

                    //Saving currentAcctMgt info to the database
                    _context.SaveChanges();
                    acctConfigLogic.SaveCurrentConfig(currentAcctMgt);

                    ViewBag.Message = "Success";
                    return RedirectToAction("UpdateCurrent", "AccountConfiguration");
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    ViewBag.Message = "Error";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }

            return View();
        }

        // GET: AccountConfiguration/UpdateCurrent
        public ActionResult UpdateCurrent()
        {
            var getCurrentAcctMgtTable = accountConfig.GetAllCurrentConfig();
            if (getCurrentAcctMgtTable.Count() == 0)
            {
                return RedirectToAction("Current", "AccountConfiguration");
            }
            else
            {
                CurrentAcctMgt currentAcctMgt = accountConfig.GetAllCurrentConfig().Single();
                if (currentAcctMgt == null)
                {
                    return HttpNotFound();
                }

                var getExpense = glCatRepo.GetName("Current Interest Expense");
                var getCotIncome = glCatRepo.GetName("Current COT Income");
                var getPayable = glCatRepo.GetName("Current Interest Payable");



                currentAcctMgt.CurrentInterestPayable = getPayable;



                currentAcctMgt.CurrentInterestExpense = getExpense;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                    c.GlCategoryId == getExpense.id),*/
                currentAcctMgt.CurrentCotIncome = getCotIncome;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
                     c.GlCategoryId == getCotIncome.id)*/
                return View(currentAcctMgt);
            }
        }

        //POST: AccountConfiguration/UpdateCurrent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCurrent([Bind(Include = "id,CurrentCreditInterestRate,CurrentMinimumBalance,CurrentCot,CurrentInterestExpenseGLId,CurrentCotIncomeGLId,CurrentInterestPayableGLId")] CurrentAcctMgt currentAcctMgt)
        {
            var getExpense = glCatRepo.GetName("Current Interest Expense");
            var getCotIncome = glCatRepo.GetName("Current COT Income");
            var getPayable = glCatRepo.GetName("Current Interest Payable");


            currentAcctMgt.CurrentInterestExpense = getExpense;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                c.GlCategoryId == getExpense.id);*/
            currentAcctMgt.CurrentCotIncome = getCotIncome;/*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
                 c.GlCategoryId == getCotIncome.id);*/
            currentAcctMgt.CurrentInterestPayable = getPayable;

            if (ModelState.IsValid)
            {
                try
                {
                    var value = Convert.ToDecimal(0.00);
                    if (currentAcctMgt.CurrentCreditInterestRate == value || currentAcctMgt.CurrentCreditInterestRate == 0 || currentAcctMgt.CurrentMinimumBalance == value || currentAcctMgt.CurrentMinimumBalance == 0
                        || currentAcctMgt.CurrentInterestExpenseGLId == null || currentAcctMgt.CurrentInterestPayableGLId == null || currentAcctMgt.CurrentCotIncomeGLId == null || currentAcctMgt.CurrentCot == value || currentAcctMgt.CurrentCot == 0)
                    {
                        currentAcctMgt.status = "Incomplete";
                    }
                    else
                    {
                        currentAcctMgt.status = "Complete";
                    }
                    
                    //Adding currentAcctMgt info to memory
                    _context.Entry(currentAcctMgt).State = EntityState.Modified;

                    //Saving currentAcctMgt info to the database
                    _context.SaveChanges();
                    acctConfigLogic.SaveCurrentConfig(currentAcctMgt);

                    ViewBag.Message = "Success";
                    return RedirectToAction("UpdateCurrent", "AccountConfiguration");
                }
                
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    ViewBag.Message = "Error";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
            return View();
        }
   

    //------------------------------Loan-----------------------------------------------------------

    // GET: AccountConfiguration/Loan
    public ActionResult Loan()
    {

        var getLoanAcctMgtTable = accountConfig.GetAllLoanConfig();
            if (getLoanAcctMgtTable.Count() > 0 && getLoanAcctMgtTable.Count() == 1)
        {
            return RedirectToAction("UpdateLoan", "AccountConfiguration");
        }
        else
        {
                //var getExpense = glCatRepo.GetName("Loan Interest Expense");
                var getIncome = glCatRepo.GetName("Loan Interest Income");
                //var getReceivable = glCatRepo.GetName("Loan Interest Receivable");


                var model = new LoanAcctMgt()
                {
                    /*LoanInterestExpense = getExpense,*/ /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                    c.GlCategoryId == getExpense.id),*/
                    LoanInterestIncome =getIncome, /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
                     c.GlCategoryId == getIncome.id),*/
                     /*LoanInterestReceivable =getReceivable*/ /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Asset" &&
                     c.GlCategoryId == getCotIncome.id)*/
                };
                return View(model);
            }
    }

    //POST: AccountConfiguration/Loan
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Loan(LoanAcctMgt loanAcctMgt)
    {
            //var getExpense = glCatRepo.GetName("Loan Interest Expense");
            var getIncome = glCatRepo.GetName("Loan Interest Income");
            //var getReceivable = glCatRepo.GetName("Loan Interest Receivable");

            /*loanAcctMgt.LoanInterestExpense = getExpense;*//*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                c.GlCategoryId == getExpense.id);*/
            loanAcctMgt.LoanInterestIncome = getIncome; /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
             c.GlCategoryId == getCotIncome.id);*/
            /*loanAcctMgt.LoanInterestReceivable = getReceivable;*/ /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Asset" &&
            c.GlCategoryId == getCotIncome.id);*/

       if (ModelState.IsValid)
        {
            try
            {
                var value = Convert.ToDecimal(0.00);
                if (loanAcctMgt.LoanDebitInterestRate == value || loanAcctMgt.LoanDebitInterestRate == 0||
                    /*loanAcctMgt.LoanInterestExpenseGLId == 0 ||*/ loanAcctMgt.LoanInterestIncomeGLId == 0 /*|| loanAcctMgt.LoanInterestReceivableGLId == 0*/)
                {
                    loanAcctMgt.status = "Incomplete";
                }
                else
                {
                    loanAcctMgt.status = "Complete";
                }

                //Adding loanAcctMgt info to memory
                _context.LoanAcctMgts.Add(loanAcctMgt);

                //Saving loanAcctMgt info to the database
                _context.SaveChanges();
                acctConfigLogic.SaveLoanConfig(loanAcctMgt);

                ViewBag.Message = "Success";
                return RedirectToAction("UpdateLoan", "AccountConfiguration");
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ViewBag.Message = "Error";

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
        return View();
    }

    // GET: AccountConfiguration/UpdateSavings
    public ActionResult UpdateLoan()
        {

            var getLoanAcctMgtTable = accountConfig.GetAllLoanConfig();
            if (getLoanAcctMgtTable.Count() == 0)
            {
                return RedirectToAction("Loan", "AccountConfiguration");
            }
            else
            {
                LoanAcctMgt loanAcctMgt = accountConfig.GetAllLoanConfig().Single();
                if (loanAcctMgt == null)
                {
                    return HttpNotFound();
                }
                //var getExpense = glCatRepo.GetName("Loan Interest Expense");
                var getIncome = glCatRepo.GetName("Loan Interest Income");
                //var getReceivable = glCatRepo.GetName("Loan Interest Receivable");


                /*loanAcctMgt.LoanInterestExpense = getExpense;*/ /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                    c.GlCategoryId == getExpense.id),*/
                loanAcctMgt.LoanInterestIncome = getIncome; /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
                     c.GlCategoryId == getCotIncome.id),*/
                /*loanAcctMgt.LoanInterestReceivable = getReceivable;*//*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Asset" &&
                    c.GlCategoryId == getCotIncome.id)*/
                return View(loanAcctMgt);
            }
        }

        //POST: AccountConfiguration/UpdateLoan
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateLoan([Bind(Include = "id,LoanDebitInterestRate,LoanInterestIncomeGLId,LoanInterestExpenseGLId")] LoanAcctMgt loanAcctMgt)
        {
            //var getExpense = glCatRepo.GetName("Loan Interest Expense");
            var getIncome = glCatRepo.GetName("Loan Interest Income");
            //var getReceivable = glCatRepo.GetName("Loan Interest Receivable");

            /*loanAcctMgt.LoanInterestExpense = getExpense;*/ /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Expenses" &&
                     c.GlCategoryId == getExpense.id);*/
            loanAcctMgt.LoanInterestIncome = getIncome; /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Income" &&
                  c.GlCategoryId == getCotIncome.id);*/
            /*loanAcctMgt.LoanInterestReceivable = getReceivable;*/ /*_context.GlAccounts.ToList().Where(c => c.assignToTeller == "" && c.mainCategory == "Asset" &&
                 c.GlCategoryId == getCotIncome.id);*/
            if (ModelState.IsValid)
            {
                try
                {
                    var value = Convert.ToDecimal(0.00);
                    if (loanAcctMgt.LoanDebitInterestRate == value || loanAcctMgt.LoanDebitInterestRate == 0 ||
                    /*loanAcctMgt.LoanInterestExpenseGLId == null ||*/ loanAcctMgt.LoanInterestIncomeGLId == null /*|| loanAcctMgt.LoanInterestReceivableGLId == null*/)
                    {
                        loanAcctMgt.status = "Incomplete";
                    }
                    else
                    {
                        loanAcctMgt.status = "Complete";
                    }

                    //Adding loanAcctMgt info to memory
                    _context.Entry(loanAcctMgt).State = EntityState.Modified;

                    //Saving loanAcctMgt info to the database
                    _context.SaveChanges();
                    acctConfigLogic.SaveLoanConfig(loanAcctMgt);

                    ViewBag.Message = "Success";
                    return RedirectToAction("UpdateLoan", "AccountConfiguration");
                }
               
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    ViewBag.Message = "Error";

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
            return View();
        }

    }
}