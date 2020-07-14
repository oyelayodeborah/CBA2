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
    [SessionRestrictLogic]
    public class CustomerAccountController : Controller
    {
        BaseRepository<CustomerAccount> baserepo = new BaseRepository<CustomerAccount>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        FinancialReportLogic frLogic = new FinancialReportLogic();
        BusinessLogic busLogic = new BusinessLogic();
        LoanCustAcctRepository loanRepo = new LoanCustAcctRepository();
        LoanCustAcctLogic loanLogic = new LoanCustAcctLogic();

        [AdminRoleRestrictLogic]
        //GET: CustomerAccount/Create
        public ActionResult Create()
        {
           var viewModel = new CustomerAccountViewModels()
            {
               Branches = _context.Branches.ToList(),
               Customers = _context.Customers.ToList(),
                };
                return View(viewModel);
        }

        //POST: CustomerAccount/Create
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "acctName,customerAccount,accType")] CustomerAccountViewModels model)
        {
            model.Branches = _context.Branches.ToList();
            model.Customers = _context.Customers.ToList();
            
                if (model != null && model.id == 0 && model.accType!=0 && model.customerAccount.customerId!=0 && model.customerAccount.branchId!=0)
                {
                    //Assigning the values gotten from the create form to the CustomerAccount model
                    CustomerAccount customerAccount = new CustomerAccount();
                    customerAccount.customerId = model.customerAccount.customerId;
                    customerAccount.branchId = model.customerAccount.branchId;
                    customerAccount.acctName = model.customerAccount.acctName;
                    customerAccount.accType = model.accType.ToString();
                    customerAccount.acctbalance = 0;
                    customerAccount.createdAt = DateTime.Now;
                    customerAccount.isLinked = false;
                    customerAccount.status = "Opened";
                    customerAccount.dailyInterestAccrued = 0;
                    customerAccount.acctNumber = customerAccountLogic.GenerateAccountNumber(customerAccount.accType, customerAccount.customerId);
                    
                    //Adding customerAccount info to memory
                    _context.CustomerAccounts.Add(customerAccount);
                    try
                    {
                        //Saving customerAccount info to the database
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        return RedirectToAction("Index", "CustomerAccount");
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
                
            else
            {
                //ViewBag.Message = "Kindly fill all the fields";
                ViewBag.Message = "Empty";
                return View(model); /*new { message="Create not successful" });*/
                //return View("Create", "CustomerAccount", model);
            }


        }
        
        //GET: CustomerAccount/Details/{id}
        [AdminRoleRestrictLogic]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CustomerAccount customerAccount = baserepo.Get(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }
            return View(customerAccount);
        }

        // GET: CustomerAccount/Edit/{id}
        [AdminRoleRestrictLogic]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            CustomerAccount customerAccount = _context.CustomerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }

            CustomerAccountViewModels model = new CustomerAccountViewModels() { id = customerAccount.id, Branches = _context.Branches.ToList() };

            return View(model);


        }

        //Post: CustomerAccount/Edit/{id}
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] CustomerAccountViewModels model)
        {
            CustomerAccount customerAccount = _context.CustomerAccounts.Find(model.id);
            model.Branches = _context.Branches.ToList();
            model.Customers = _context.Customers.ToList();

                try
                {

                    //Adding customerAccount info to memory
                    _context.Entry(customerAccount).State = EntityState.Modified;

                    //Updating customerAccount info to the database
                    _context.SaveChanges();
                TempData["Message"] = "Success";
                    return RedirectToAction("Index", "CustomerAccount");
                }
                catch (DbEntityValidationException ex)
                {
                    ViewBag.Message = "Error";

                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                }
            

        }

        //GET: CustomerAccount/Close/{id}
        [AdminRoleRestrictLogic]
        [HttpGet]
        public ActionResult Close(int? id)
        {
            customerAccountLogic.Closed(id);
            return RedirectToAction("Index", "CustomerAccount");
        }

        //GET: CustomerAccount/Open/{id}
        [AdminRoleRestrictLogic]
        [HttpGet]
        public ActionResult Open(int? id)
        {
            customerAccountLogic.Opened(id);
            return RedirectToAction("Index","CustomerAccount");
        }


        //GET: CustomerAccount/Post/{id}
        [TellerRoleRestrictLogic]
        [HttpGet]
        public ActionResult Post(int? id)
        {
            return RedirectToAction
                ("Create","TellerPosting",new { id });
        }

        // GET: CustomerAccount
        [SessionRestrictLogic]
        public ActionResult Index()
        {
            var customerAccount = baserepo.GetAll();
            return View(customerAccount.ToList());
            //return View(_context.CustomerAccounts.ToList());
        }

        //GET: CustomerAccount/OpenLoanAccount
        [AdminRoleRestrictLogic]
        public ActionResult OpenLoanAccount(int? id)
        {
            TempData["message"] = "";
            return View();
        }

        //GET: CustomerAccount/OpenLoanAccount
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenLoanAccount(LoanCustAcct model)
        {
           CustomerAccount customerAccount = _context.CustomerAccounts.Find(model.Id);
            model.ServicingAccountId = model.Id;
            model.ServicingAccounts = _context.CustomerAccounts.Where(c => c.status == "Opened").ToList();
            var CheckIfAccountExist = loanLogic.LoanAccountExist(model.ServicingAccountId);

            AccountConfigurationRepository loanAccountRepo = new AccountConfigurationRepository();
            CustomerAccountRepository custAccountRepo = new CustomerAccountRepository();
            CustomerAccountLogic custAccountLogic = new CustomerAccountLogic();
            GlAccountRepository glAccRepo = new GlAccountRepository();
            var getloanconfig = loanAccountRepo.GetAllLoanConfig();
            TempData["message"] = "";
            if (getloanconfig.Count() != 0)
                {
                    var returnVal = getloanconfig.Single();
                    model.InterestRate = returnVal.LoanDebitInterestRate;
                    model.StartDate = DateTime.Now;
                    model.EndDate = DateTime.Now.AddDays((Convert.ToInt16(model.DurationInMonths)) * 30);
                    model.Interest = model.LoanAmount * (model.InterestRate / 100) * ((Convert.ToDecimal(model.DurationInMonths) / 12));
                    model.LoanInterestRemaining = model.Interest;

                    var interestreduction = model.Interest / ((Convert.ToDecimal(model.DurationInMonths)) * 30);
                    model.LoanInterestReduction =interestreduction;
                    model.LoanAmountRemaining = model.LoanAmount;

                    var amountreduction = model.LoanAmount / ((Convert.ToDecimal(model.DurationInMonths)) * 30);
                    model.LoanAmountReduction = amountreduction;
                    model.status = "UnPaid";
                    customerAccount = custAccountRepo.Get(model.ServicingAccountId);
                    customerAccount.isLinked = true;

                    var getloanAccount = glAccRepo.GetLoanAccount();
                    GlAccount loanAccount = glAccRepo.Get(getloanAccount.id);
                    model.DaysCount = 0;
                    model.termsOfLoan = TermsOfLoan.Fixed;
                
                    var isOpened = custAccountRepo.GetOpenedAccount(customerAccount.acctNumber);
                if (isOpened == null)
                {
                    TempData["message"] = "Closed";
                    return View(model);
                }
                else
                {
                    if (CheckIfAccountExist != null)
                    {
                        LoanCustAcct loan = loanRepo.Get(CheckIfAccountExist.Id);
                        var UnPaid = loanRepo.GetUnPaid(CheckIfAccountExist.AccountNumber);

                        if (UnPaid != null)
                        {
                            TempData["message"] = "UnPaid";
                            return View(model);
                        }
                        else
                        {
                            loan.LoanAmount = model.LoanAmount;
                            loan.LoanAmountReduction = model.LoanAmountReduction;
                            loan.LoanAmountRemaining = model.LoanAmountRemaining;
                            loan.LoanInterestReduction = model.LoanInterestReduction;
                            loan.LoanInterestRemaining = model.LoanInterestRemaining;
                            loan.Interest = model.Interest;
                            loan.InterestRate = model.InterestRate;
                            loan.StartDate = model.StartDate;
                            loan.EndDate = model.EndDate;
                            loan.DaysCount = model.DaysCount;
                            loan.status = model.status;
                            loan.DurationInMonths = model.DurationInMonths;
                            loan.termsOfLoan = model.termsOfLoan;
                            var ln = loan;
                            loanRepo.Update(loan);
                            if (loanAccount != null)
                            {
                                busLogic.CreditCustomerAccount(customerAccount, model.LoanAmount);

                                busLogic.DebitGl(loanAccount, model.LoanAmount);
                                TempData["message"] = "Success";
                                return RedirectToAction("ViewLoanAccount", "CustomerAccount");

                            }
                            else
                            {
                                TempData["message"] = "Gl Account";
                                return View(model);
                            }

                            //List<LoanCustAcct> loanAcct = new List<LoanCustAcct>();
                            //loanAcct.Add(model);
                            //loanRepo.Update(loanAcct);
                            //custAccountRepo.Update(customerAccount);
                            //glAccRepo.Update(loanAccount);

                        }
                    }
                    else
                    {
                        if (loanAccount != null)
                        {
                            busLogic.CreditCustomerAccount(customerAccount, model.LoanAmount);
                            busLogic.DebitGl(loanAccount, model.LoanAmount);
                            model.AccountNumber = custAccountLogic.GenerateAccountNumber("Loan", model.Id);
                            model.Id = 0;
                            loanRepo.Save(model);
                            //custAccountRepo.Update(customerAccount);
                            //glAccRepo.Update(loanAccount);
                            //custAccountRepo.Update(customerAccount);
                            TempData["message"] = "Success";
                            return RedirectToAction("ViewLoanAccount", "CustomerAccount");

                        }
                        else
                        {
                            TempData["message"] = "Gl Account";
                            return View(model);
                        }

                    }
                }
            }
            else {
                TempData["message"] = "Error";
                return View();
            }
          }

        [AdminRoleRestrictLogic]
        public ActionResult TakeLoan()
        {
            LoanCustAcct model = new LoanCustAcct();
            model.ServicingAccounts = _context.CustomerAccounts.Where(c => c.status == "Opened").ToList();

            TempData["message"] = "";
            return View(model);
        }
        //GET: CustomerAccount/ViewLoanAccount
        [AdminRoleRestrictLogic]
        public ActionResult ViewLoanAccount()
        {
            return View(_context.LoanCustAccts.ToList());
        }


        //GET: CustomerAccount/LoanDetails/{id}
        [AdminRoleRestrictLogic]
        public ActionResult LoanDetails(int? id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }
            LoanCustAcct loanCustAcct = _context.LoanCustAccts.Find(id);
            if (loanCustAcct == null)
            {
                return HttpNotFound();
            }
            return View(loanCustAcct);
        }

        

    }
}

    