using System;
using System.Linq;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Logic;

namespace MyCBA.Controllers
{
    public class HomeController : Controller
    {
        UserLogic userLogic = new UserLogic();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["id"] != null && userLogic.IsRoleCheck(Session["role"]) == true)
            {
                ApplicationDbContext _context = new ApplicationDbContext();
                ViewBag.teller = _context.Tellers.ToList().Count();
                ViewBag.tellerPosting = _context.TellerPostings.ToList().Count();
                ViewBag.glPosting = _context.GlPostings.ToList().Count();
                ViewBag.branch = _context.Branches.ToList().Count();
                ViewBag.role = _context.Roles.ToList().Count();
                ViewBag.glCategory = _context.GlCategories.ToList().Count();
                ViewBag.glAccounts = _context.GlAccounts.ToList().Count();
                ViewBag.customer = _context.Customers.ToList().Count();
                ViewBag.customerAccount = _context.CustomerAccounts.ToList().Count() + _context.LoanCustAccts.ToList().Count();
                ViewBag.unpaidLoan = _context.LoanCustAccts.Where(c => c.status == "UnPaid").ToList().Count();
                var id = Convert.ToInt32(Session["id"]);
                ViewBag.transaction = _context.TellerPostings.Where(c => c.userId == id).ToList().Count();
                ViewBag.transactionNum = _context.Transactions.ToList().Count();
                if (ViewBag.transactionNum != 0)
                {
                    ViewBag.transactionNum = Convert.ToInt32(ViewBag.transactionNum) / 2;
                }
                ViewBag.deposit = _context.TellerPostings.Where(c => c.userId == id && c.postingType == "Deposit").ToList().Count();
                ViewBag.withdrawal = _context.TellerPostings.Where(c => c.userId == id && c.postingType == "Withdrawal").ToList().Count();
                var noConfig = _context.AccountConfigurations.Where(c => c.FinancialDate == null).FirstOrDefault();
                if (noConfig != null)
                {
                    ViewBag.NoConfig = noConfig.FinancialDate;
                    var openConfig = _context.AccountConfigurations.Where(c => c.IsBusinessOpen == true || c.IsBusinessOpen == false).FirstOrDefault();
                    if (openConfig != null)
                    {
                        ViewBag.OpenConfig = openConfig.IsBusinessOpen;
                    }
                    
                    var configStatus = _context.AccountConfigurations.Where(c => c.status == "Complete"|| c.status == "Incomplete").FirstOrDefault();
                    if (configStatus != null)
                    {
                        ViewBag.ConfigStatus = configStatus.status;
                    }
                    
                }
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Home");
            }

        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login","Account");
        }
    }
}
