using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class TellerController:Controller
    {
        BaseRepository<Teller> baserepo = new BaseRepository<Teller>(new ApplicationDbContext());
        BaseRepository<GlAccount> baserepo2 = new BaseRepository<GlAccount>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        TellerLogic tellerLogic = new TellerLogic();

        // GET: Teller
        public ActionResult Index()
        {
            return View(_context.Tellers.ToList());
        }

        //GET: Teller/Details/{id}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Teller teller = baserepo.Get(id);
            if (teller == null)
            {
                return HttpNotFound();
            }
            return View(teller);
        }

        //GET: Teller/AssignTillAccount

        public ActionResult AssignTillAccount()
        {
            RoleRepository roleRepo = new RoleRepository();
            var role = roleRepo.GetByName("Teller").Single();
            var viewModel = new TellerViewModels()
            {
                Tellers = _context.Users.ToList().Where(c => c.roleId == role.id && c.IsAssigned=="false"),
                TillAccounts = _context.GlAccounts.ToList().Where(c=>c.assignToTeller=="false")

            };
            return View(viewModel);
        }

        //POST: Teller/AssignTillAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTillAccount(TellerViewModels tellerViewModels)
        {
            RoleRepository roleRepo = new RoleRepository();
            GlAccountRepository glAcctRepo = new GlAccountRepository();
            var role = roleRepo.GetByName("Teller").Single();
            tellerViewModels.Tellers = _context.Users.ToList().Where(c => c.roleId == role.id && c.IsAssigned == "false");
            tellerViewModels.TillAccounts = _context.GlAccounts.ToList().Where(c => c.assignToTeller == "false");

            if (ModelState.IsValid)
            {
                Teller teller = new Teller();
                teller.TillAccountId = tellerViewModels.TillAccountId;
                teller.userId = tellerViewModels.userId;
                var findGlAccountDetails = glAcctRepo.Get(teller.TillAccountId);//_context.GlAccounts.Where(a => a.Name == teller.tillAccount).FirstOrDefault();

                teller.tillAccountBalance = findGlAccountDetails.acountBalance;
                
                GlAccount glAccount = new GlAccount();
                glAccount = _context.GlAccounts.Find(findGlAccountDetails.id);
                
                glAccount.assignToTeller = "true";
                    
                var findUserDetails = _context.Users.Where(a => a.id == teller.userId).FirstOrDefault();
                User user = new User();
                user = _context.Users.Find(findUserDetails.id);
                user.IsAssigned = "true";

                var CheckTeller = tellerLogic.IsTellerAssigned(teller.userId);
                if (!CheckTeller)
                {
                  TempData["Message"] = "Teller is already assigned";
                  return View(tellerViewModels);
                }
                else
                {
                    try
                    {
                        _context.Entry(glAccount).State = EntityState.Modified;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                        baserepo.Save(teller);
                        ViewBag.Message = "Success";


                        return RedirectToAction("Index", "Teller");
                    }
                    catch (Exception)
                    {
                        TempData["Message"] = "Teller was not assigned successfully";
                        return View(tellerViewModels);
                    }
                    
                }
            }
                return View(tellerViewModels);
        }
    }
}
