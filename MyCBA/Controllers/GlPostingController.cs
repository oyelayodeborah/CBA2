using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;
using MyCBA.ViewModels;

namespace MyCBA.Controllers
{
    [AdminRoleRestrictLogic]
    public class GlPostingController:Controller
    {
        BaseRepository<GlPosting> baserepo = new BaseRepository<GlPosting>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        GlPostingLogic glPostingLogic = new GlPostingLogic();
        BusinessLogic busLogic = new BusinessLogic();

        //GET: GlPosting/Create
        public ActionResult Create()
        {
            var viewModel = new GlPostingViewModels()
            {
                GlAccounts = _context.GlAccounts.ToList().Where(c=>c.assignToTeller!="false")
            };
            return View(viewModel);
        }

        //POST: GlPosting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GlPostingViewModels model)
        {
            model.GlAccounts = _context.GlAccounts.ToList().Where(c => c.assignToTeller != "false");
            if (ModelState.IsValid)
            {
                GlPosting glPosting = new GlPosting();

                glPosting.userId = (int)Session["id"];
                glPosting.TransactionDate = DateTime.Now;
                glPosting.Amount = model.Amount;
                glPosting.Narration = model.Narration;
                glPosting.GlAccountToDebitId = model.GlAccountToDebit;
                glPosting.GlAccountToCreditId = model.GlAccountToCredit;

                var FindDebitAccount = _context.GlAccounts.Where(a => a.id == model.GlAccountToDebit).FirstOrDefault();
                glPosting.GlAccountToDebitCode = FindDebitAccount.Code;
                TellerRepository tellerRepo = new TellerRepository();


                var FindCreditAccount = _context.GlAccounts.Where(a => a.id == model.GlAccountToCredit).FirstOrDefault();
                glPosting.GlAccountToCreditCode = FindCreditAccount.Code;


                GlAccount gldebitAccount = new GlAccount();
                gldebitAccount = _context.GlAccounts.Find(FindDebitAccount.id);

                GlAccount glcreditAccount = new GlAccount();
                glcreditAccount = _context.GlAccounts.Find(FindCreditAccount.id);
                if (glcreditAccount == gldebitAccount)
                {
                    ViewBag.Message = "Same Account";
                    return View(model);
                }
                else if (glPostingLogic.IsDebitable(gldebitAccount, glPosting.Amount)==false)
                {
                    ViewBag.Message = "Insufficient Balance";
                    return View(model);
                }
                else if (glPostingLogic.IsCreditable(glcreditAccount, glPosting.Amount)==false)
                {
                    ViewBag.Message = "Insufficient Balance";
                    return View(model);
                }
                else
                {
                    busLogic.CreditGl(glcreditAccount, glPosting.Amount);
                    busLogic.DebitGl(gldebitAccount, glPosting.Amount);

                    GlAccountRepository glAcct = new GlAccountRepository();
                    var debitval = glAcct.GetByName(FindDebitAccount.Name);
                    if (debitval != null)
                    {
                        var contain = debitval.Name.ToLower().Contains("till");
                        if (contain == true)
                        {
                            var getDebitTillAccount = tellerRepo.GetByTillAccount(FindDebitAccount.id);
                            Teller debitTeller = tellerRepo.Get(getDebitTillAccount.id);
                            if (getDebitTillAccount != null)
                            {
                                var debitbal = getDebitTillAccount.tillAccountBalance + glPosting.Amount;
                                debitTeller.tillAccountBalance = debitbal;
                                tellerRepo.Update(debitTeller);
                            }
                        }
                    }
                    var creditVal = glAcct.GetByName(FindCreditAccount.Name);
                    if (creditVal != null)
                    {
                        var contain = creditVal.Name.ToLower().Contains("till");
                        if (contain == true)
                        {
                            var getCreditTillAccount = tellerRepo.GetByTillAccount(FindCreditAccount.id);
                            Teller creditTeller = tellerRepo.Get(getCreditTillAccount.id);
                            if (getCreditTillAccount != null)
                            {
                                var creditbal = getCreditTillAccount.tillAccountBalance + glPosting.Amount;
                                creditTeller.tillAccountBalance = creditbal;
                                tellerRepo.Update(creditTeller);
                            }
                        }
                    }




                    glPosting.status = "Successful";
                    baserepo.Save(glPosting);
                    _context.Entry(gldebitAccount).State = EntityState.Modified;
                    _context.Entry(glcreditAccount).State = EntityState.Modified;
                    _context.SaveChanges();


                    try
                    {
                        if (glPosting.status == "Successful")
                        {
                            TempData["Message"] = "Success";
                            return RedirectToAction("Index", "GlPosting");
                        }
                        else
                        {
                            TempData["Message"] = "Success";
                            baserepo.Save(glPosting);
                            return RedirectToAction("Index", "GlPosting");
                        }
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

            return View(model);
        }



        public ActionResult Index()
        {
            return View(_context.GlPostings.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            GlPosting glposting = baserepo.Get(id);
            if (glposting == null)
            {
                return HttpNotFound();
            }
            return View(glposting);
        }

        
    }
}
