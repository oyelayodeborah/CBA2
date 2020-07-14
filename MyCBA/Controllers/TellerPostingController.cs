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
    public class TellerPostingController : Controller
    {
        FinancialReportLogic frLogic = new FinancialReportLogic();
        BusinessLogic busLogic = new BusinessLogic();

        //GET: TellerPosting/Index
        [AdminRoleRestrictLogic]
        public ActionResult Index()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var postings = _context.TellerPostings.ToList();
             return View(postings == null ? new List<TellerPosting>() : postings);
        }

        // GET: TellerPosting/UserPosts
        [TellerRoleRestrictLogic]
        public ActionResult UserPosts()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            TellerRepository tellerRepo = new TellerRepository();
            GlAccountRepository glAcctRepo = new GlAccountRepository();
            //List<TellerPosting> postings;
            //postings = _context.TellerPostings.ToList();
            // return View(_context.TellerPostings.ToList());//View(postings == null ? new List<TellerPosting>() : postings);
            TempData["message"] = "";
            if (Session["tillbalance"] == null || Session["currenttillbalance"] == null || Session["InsufficientTill"] == null
                || Session["Insufficient"] == null)
            {
                int id = (int)Session["id"];
                var postings = _context.TellerPostings.Where(c => c.userId == id).ToList();
                if (postings == null)
                {
                    return View();
                }
                else {
                    return View(postings == null ? new List<TellerPosting>() : postings);
                }
                //return View(_context.TellerPostings.Where(c=>c.userId==(int)Session["id"]).ToList());
            }
            else
            {
                var tillbalance = Convert.ToDecimal(Session["tillbalance"]);
                var curbalance = Convert.ToDecimal(Session["currenttillbalance"]);
                TempData["message"] = "";

                if (Session["Insufficient"].ToString()!=null)
                {
                    TempData["message"] = "Insufficient";

                }
                if (Session["InsufficientTill"].ToString() != null)
                {
                    TempData["message"] = "InsufficientTill";

                }
                if (curbalance > tillbalance)
                {
                    TempData["message"] = "Buy Cash";
                }
                else if (curbalance < tillbalance)
                {
                    TempData["message"] = "Sell Cash";
                }
                else
                {
                    TempData["message"] = "";
                }
                int id = (int)Session["id"];
                var postings = _context.TellerPostings.Where(c => c.userId == id).ToList();
                if (postings == null)
                { 
                    return View();
                }
                else
                {
                    return View(postings == null ? new List<TellerPosting>() : postings);
                }
                //return View(_context.TellerPostings.Where(c=>c.userId== (int)Session["id"]).ToList());
            }
        }

        //POST:TellerPosting/BuyCash
        [TellerRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyCash()
        {
            TellerRepository tellerRepo = new TellerRepository();
            GlAccountRepository glAcctRepo = new GlAccountRepository();

            var Amount = Convert.ToDecimal(Request.Form["amount"]);
            var findTellerDetails = tellerRepo.GetByUser((int)Session["id"]);
            TempData["message"] = "";

            if (findTellerDetails == null)
            {
                TempData["message"] = "No Account";
                return RedirectToAction("UserPosts", "TellerPosting");

            }
            else
            {
                var findTillAccountDetails = glAcctRepo.Get(findTellerDetails.TillAccountId);
                var findVaultAccountDetails = glAcctRepo.GetVault();
                decimal tellerbalance = 0;
                GlAccount vaultAccount = new GlAccount();
                GlAccount tillAccount = new GlAccount();
                Teller teller = new Teller();
                GlPosting glPosting = new GlPosting();
                var appDBContext = new ApplicationDbContext();
                vaultAccount = glAcctRepo.Get(findVaultAccountDetails.id);
                tillAccount = glAcctRepo.Get(findTillAccountDetails.id);
                decimal vaultbal = findVaultAccountDetails.acountBalance;
                if (vaultbal < Amount)
                {
                    Session["vaultbalance"] = findVaultAccountDetails.acountBalance;
                    TempData["message"] = "VaultError";
                    return RedirectToAction("UserPosts", "TellerPosting");
                }
                else
                {
                    if (findVaultAccountDetails.acountBalance >= Amount)
                    {

                        Session["tillbalance"] = findTillAccountDetails.acountBalance;
                        tellerbalance = findTellerDetails.tillAccountBalance + Amount;
                        //tillaccountbalance = findTillAccountDetails.acountBalance - Amount;
                        busLogic.CreditGl(vaultAccount, Amount);
                        //vaultbalance = findVaultAccountDetails.acountBalance + Amount;
                        busLogic.DebitGl(tillAccount, Amount);

                        teller = tellerRepo.Get(findTellerDetails.id);
                        teller.tillAccountBalance = tellerbalance;

                        //tillAccount.acountBalance = tillaccountbalance;

                        //vaultAccount.acountBalance = vaultbalance;

                        glPosting.Amount = Amount;
                        glPosting.GlAccountToCreditId = findVaultAccountDetails.id;
                        glPosting.GlAccountToCreditCode = findVaultAccountDetails.Code;
                        glPosting.GlAccountToDebitId = findTillAccountDetails.id;
                        glPosting.GlAccountToDebitCode = findTillAccountDetails.Code;
                        glPosting.Narration = "Buying Cash";
                        glPosting.report = "Successful";
                        glPosting.status = "Successful";
                        glPosting.TransactionDate = DateTime.Now;
                        glPosting.userId = (int)Session["id"];

                        tellerRepo.Update(teller);
                        glAcctRepo.Update(tillAccount);
                        glAcctRepo.Update(vaultAccount);

                        appDBContext.GlPostings.Add(glPosting);
                        appDBContext.SaveChanges();

                        Session["currenttillbalance"] = tillAccount.acountBalance;
                        ViewBag.Message = "Success";
                        return RedirectToAction("UserPosts", "TellerPosting");
                    }
                    else
                    {
                        TempData["message"] = "Error";
                        Session["Insufficient"] = "Insufficient Balance to buy from";
                        return RedirectToAction("UserPosts", "TellerPosting");
                    }

                }
            }
        }

        //POST:TellerPosting/SellCash
        [TellerRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellCash()
        {
            TellerRepository tellerRepo = new TellerRepository();
            GlAccountRepository glAcctRepo = new GlAccountRepository();

            var Amount = Convert.ToDecimal(Request.Form["amount"]);
            var findTellerDetails = tellerRepo.GetByUser((int)Session["id"]);
            TempData["message"] = "";

            if (findTellerDetails == null)
            {
                TempData["message"] = "No Account";
                return RedirectToAction("UserPosts", "TellerPosting");

            }
            else
            {
                var findTillAccountDetails = glAcctRepo.Get(findTellerDetails.TillAccountId);
                var findVaultAccountDetails = glAcctRepo.GetVault();
                decimal tellerbalance = 0;
                GlAccount vaultAccount = new GlAccount();
                GlAccount tillAccount = new GlAccount();
                Teller teller = new Teller();
                GlPosting glPosting = new GlPosting();
                var appDBContext = new ApplicationDbContext();
                tillAccount = glAcctRepo.Get(findTillAccountDetails.id);
                vaultAccount = glAcctRepo.Get(findVaultAccountDetails.id);
                decimal tillBal = tillAccount.acountBalance;
                if (tillBal < Amount)
                {
                    TempData["message"] = "InsufficientTill";
                    Session["InsufficientTill"] = "Your Balance is insufficient";
                    return RedirectToAction("UserPosts", "TellerPosting");
                }
                else
                {
                    if (findTillAccountDetails.acountBalance >= Amount)
                    {
                        Session["tillbalance"] = findTillAccountDetails.acountBalance;

                        tellerbalance = findTellerDetails.tillAccountBalance - Amount;
                        //tillaccountbalance = findTillAccountDetails.acountBalance - Amount;
                        busLogic.CreditGl(tillAccount, Amount);
                        //vaultbalance = findVaultAccountDetails.acountBalance + Amount;
                        busLogic.DebitGl(vaultAccount, Amount);

                        teller = tellerRepo.Get(findTellerDetails.id);
                        teller.tillAccountBalance = tellerbalance;

                        //tillAccount = glAcctRepo.Get(findTillAccountDetails.id);
                        //tillAccount.acountBalance = tillaccountbalance;

                        //vaultAccount = glAcctRepo.Get(findVaultAccountDetails.id);
                        //vaultAccount.acountBalance = vaultbalance;

                        glPosting.Amount = Amount;
                        glPosting.GlAccountToCreditId = findTillAccountDetails.id;
                        glPosting.GlAccountToCreditCode = findTillAccountDetails.Code;
                        glPosting.GlAccountToDebitId = findVaultAccountDetails.id;
                        glPosting.GlAccountToDebitCode = findVaultAccountDetails.Code;
                        glPosting.Narration = "Selling Cash";
                        glPosting.report = "Successful";
                        glPosting.status = "Successful";
                        glPosting.TransactionDate = DateTime.Now;
                        glPosting.userId = (int)Session["id"];

                        tellerRepo.Update(teller);
                        glAcctRepo.Update(tillAccount);
                        glAcctRepo.Update(vaultAccount);

                        appDBContext.GlPostings.Add(glPosting);
                        appDBContext.SaveChanges();
                        Session["currenttillbalance"] = tillAccount.acountBalance;

                        ViewBag.Message = "Success";
                        return RedirectToAction("UserPosts", "TellerPosting");
                    }
                    else
                    {
                        TempData["message"] = "Error";
                        Session["InsufficientTill"] = "Your Balance is insufficient";
                        return RedirectToAction("UserPosts", "TellerPosting");
                    }
                }
            }
        }

        //GET: TellerPosting/Create
        [TellerRoleRestrictLogic]
        public ActionResult Post()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            TempData["message"] = "";
            TellerPostingViewModels model = new TellerPostingViewModels()
            { CustomerAccount = _context.CustomerAccounts.Where(c => c.status == "Opened").ToList() };
            return View(model);
        }

        //GET: TellerPosting/Create
        [TellerRoleRestrictLogic]
        public ActionResult Create(int? id)
        {
            CustomerAccountRepository custAcctRepo = new CustomerAccountRepository();
            if (id == null)
            {
                return HttpNotFound();
            }

            CustomerAccount customerAccount = custAcctRepo.Get(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }
            else
            {
                var isOpened = custAcctRepo.GetOpenedAccount(customerAccount.acctNumber);
                if (isOpened != null)
                {
                    TempData["message"] = "";
                    TellerPostingViewModels model = new TellerPostingViewModels()
                    {
                        customerAcctNum = customerAccount.acctNumber,
                        customerName = customerAccount.acctName,
                        customerId=customerAccount.customerId
                    };

                    return View(model);
                }
                else
                {
                    TempData["message"] = "Closed";
                    return View(id);
                }
            }
        }

        //POST: TellerPosting/Create
        [TellerRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TellerPostingViewModels models)
        {
            var appDBContext = new ApplicationDbContext();

            GlAccount glAccount = new GlAccount();
            GlAccount cotglAccount = new GlAccount();
            CustomerAccount customerAccount = new CustomerAccount();


            TellerPostingLogic tellerPostingLogic = new TellerPostingLogic();
            //SavingsAccountMgtRepository acctConfigRepo = new SavingsAccountMgtRepository();
            AccountConfigurationRepository acctConfigRepo = new AccountConfigurationRepository();

            Teller teller = new Teller();
            //CurrentAccountMgtRepository acctConfigRepo = new CurrentAccountMgtRepository();
            GlAccountRepository glAcctRepo = new GlAccountRepository();
            TellerRepository tellerRepo = new TellerRepository();
            CustomerAccountRepository custAcctRepo = new CustomerAccountRepository();
            CustomerAccount customerAccountt = new CustomerAccount();
            if (models.id != 0)
            {
               customerAccountt = appDBContext.CustomerAccounts.Find(models.id);
               models.customerAcctNum = customerAccountt.acctNumber;

            }
            else
            {
                var custAccountt = custAcctRepo.GetByAcctNum(models.customerAcctNum);
                customerAccountt = appDBContext.CustomerAccounts.Find(custAccountt.id);
                models.customerId = customerAccountt.customerId;

            }
            models.customerName = customerAccountt.acctName;
            var getteller = Session["email"].ToString();

            TellerPosting tellerPosting = new TellerPosting();
            tellerPosting.customerId = models.customerId;
            tellerPosting.customerAcctNum = models.customerAcctNum;
            tellerPosting.TransactionDate = DateTime.Now;
            tellerPosting.Amount = models.Amount;
            tellerPosting.Narration = models.Narration;
            tellerPosting.postingType = models.postingType.ToString();
            tellerPosting.userId = Convert.ToInt32(Session["id"]);


            //var findCustomerAccountDetails = custAcctRepo.GetByAcctNum(tellerPosting.customerAcctNum);
            var findCustomerAccountDetails = custAcctRepo.GetByAcctNum(tellerPosting.customerAcctNum);
           
             var findTellerDetails = tellerRepo.GetByUser(tellerPosting.userId);
            TempData["message"] = "";

            if (findTellerDetails == null)
            {
                TempData["message"] = "No Account";
                return RedirectToAction("UserPosts", "TellerPosting");

            }
            else
            {
                var findGlAccountDetails = glAcctRepo.Get(findTellerDetails.TillAccountId);
                var findCurrentAccountDetails = acctConfigRepo.GetAllCurrentConfig().Single();
                var findSavingsAccountDetails = acctConfigRepo.GetAllSavingsConfig().Single();
                var findGlAccountCOTDetails = glAcctRepo.Get(findCurrentAccountDetails.CurrentCotIncomeGLId);
                decimal tellerbalance = 0;
                decimal cotbalance = 0;
                //decimal withdrawableamount = 0;
                bool withdrawable = false;
                decimal curlien = 0;
                decimal savlien = 0;
                tellerPosting.status = "Successful";

                customerAccount = custAcctRepo.Get(findCustomerAccountDetails.id);
                glAccount = glAcctRepo.Get(findGlAccountDetails.id);
                cotglAccount = glAcctRepo.Get(findGlAccountCOTDetails.id);


                if (tellerPosting.postingType == "Deposit")
                {
                    //custbalance = findCustomerAccountDetails.acctbalance + tellerPosting.Amount;
                    busLogic.CreditCustomerAccount(customerAccount, tellerPosting.Amount);
                    tellerbalance = findTellerDetails.tillAccountBalance + tellerPosting.Amount;
                    //glbalance = findGlAccountDetails.acountBalance + tellerPosting.Amount;
                    busLogic.DebitGl(glAccount, tellerPosting.Amount);

                    
                }
                else//Withdrawal
                {
                    if (findGlAccountDetails.acountBalance >= tellerPosting.Amount)
                    {
                        if (findCustomerAccountDetails.accType == "Savings")
                        {
                            withdrawable = tellerPostingLogic.Withdrawable(findCustomerAccountDetails.acctbalance, findTellerDetails.tillAccountBalance,
                          findCustomerAccountDetails.accType, findCurrentAccountDetails.CurrentCot,
                          findSavingsAccountDetails.SavingsMinimumBalance, curlien, tellerPosting.Amount);

                            if (withdrawable == true)
                            {
                                //custbalance = findCustomerAccountDetails.acctbalance - tellerPosting.Amount;
                                busLogic.DebitCustomerAccount(customerAccount, tellerPosting.Amount);

                                tellerbalance = findTellerDetails.tillAccountBalance - tellerPosting.Amount;

                                //glbalance = findGlAccountDetails.acountBalance - tellerPosting.Amount;
                                busLogic.CreditGl(glAccount, tellerPosting.Amount);

                                ViewBag.Message = "Success";
                            }
                            else
                            {
                                TempData["message"] = "Error";
                                return View(models);
                            }
                        }

                        else
                        {
                            withdrawable = tellerPostingLogic.Withdrawable(findCustomerAccountDetails.acctbalance, findTellerDetails.tillAccountBalance,
                          findCustomerAccountDetails.accType, findCurrentAccountDetails.CurrentCot, savlien,
                          findCurrentAccountDetails.CurrentMinimumBalance, tellerPosting.Amount);

                            if (withdrawable == true)
                            {
                                tellerbalance = findTellerDetails.tillAccountBalance - tellerPosting.Amount;
                                //glbalance = findGlAccountDetails.acountBalance - tellerPosting.Amount;
                                if (cotglAccount == null)
                                {
                                    TempData["message"] = "GL Account";
                                    return View(models);
                                }
                                else
                                {
                                    busLogic.CreditGl(glAccount, tellerPosting.Amount);

                                    var cot = tellerPostingLogic.CalculateCOT(tellerPosting.Amount, findCurrentAccountDetails.CurrentCot);
                                    cotbalance = findGlAccountCOTDetails.acountBalance + cot;
                                    busLogic.CreditGl(cotglAccount, cot);

                                    //custbalance = findCustomerAccountDetails.acctbalance - tellerPosting.Amount - cotbalance;
                                    var custBal = tellerPosting.Amount + cotbalance;
                                    busLogic.DebitCustomerAccount(customerAccount, tellerPosting.Amount);
                                    busLogic.DebitCustomerAccount(customerAccount, cotbalance);


                                    ViewBag.Message = "Success";
                                }
                            }
                            else
                            {
                                TempData["message"] = "Error";
                                return View(models);
                            }
                        }
                    }
                    else
                    {
                        TempData["message"] = "TillError";
                        return View(models);
                    }
                }

                teller = tellerRepo.Get(findTellerDetails.id);
                teller.tillAccountBalance = tellerbalance;

                //glAccount = glAcctRepo.Get(findGlAccountDetails.id);
                //glAccount.acountBalance = glbalance;

                if (findCustomerAccountDetails.accType == "Current" && tellerPosting.postingType=="Deposit")
                {
                    //cotglAccount.acountBalance = cotbalance;

                    COTLog cotlog = new COTLog();

                    cotlog.customerAcctNum = findCustomerAccountDetails.acctNumber;
                    cotlog.customerId = findCustomerAccountDetails.id;
                    cotlog.TransactionDate = DateTime.Now;
                    cotlog.GlAccountToCreditId = findGlAccountCOTDetails.id;
                    cotlog.GlAccountToCreditCode = findGlAccountCOTDetails.Code;

                    glAcctRepo.Update(cotglAccount);
                    appDBContext.COTLogs.Add(cotlog);
                    appDBContext.SaveChanges();
                }

                //CustomerAccount customerAccount = new CustomerAccount();
                //customerAccount = custAcctRepo.Get(findCustomerAccountDetails.id);
                //customerAccount.acctbalance = custbalance;


                try
                {
                    BaseRepository<TellerPosting> baserepo = new BaseRepository<TellerPosting>(appDBContext);
                    baserepo.Save(tellerPosting);
                    glAcctRepo.Update(glAccount);
                    custAcctRepo.Update(customerAccount);
                    tellerRepo.Update(teller);


                    return RedirectToAction("UserPosts", "TellerPosting");
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
            }
            

        }

        //GET: TellerPosting/Details/{id}
        [TellerRoleRestrictLogic]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            BaseRepository<TellerPosting> baserepo = new BaseRepository<TellerPosting>(new ApplicationDbContext());
            TellerPosting tellerposting = baserepo.Get(id);
            if (tellerposting == null)
            {
                return HttpNotFound();
            }
            return View(tellerposting);
        }
    }
}