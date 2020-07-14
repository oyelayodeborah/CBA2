using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
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
    public class GlAccountController:Controller
    {
        BaseRepository<GlAccount> baserepo = new BaseRepository<GlAccount>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        GlAccountLogic glAccountLogic = new GlAccountLogic();
        GlCategoryRepository glCatRepo = new GlCategoryRepository();

        //GET: GlAccount/Create
        public ActionResult Create()
        {
            var viewModel = new GlAccountViewModels()
            {
                Branches = _context.Branches.ToList(),
                GlCategories = _context.GlCategories.ToList()
            };
            return View(viewModel);
        }

        //POST: GlAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GlAccountViewModels model)
        {

            model.Branches = _context.Branches.ToList();
            model.GlCategories = _context.GlCategories.ToList();
            if (ModelState.IsValid)
            {
                if (model != null && model.id == 0)
                {
                    var itExist = glAccountLogic.IsDetailsExist(model.GlAccount.Name);
                    var message = "";
                    if (itExist==false)
                    {
                        message= "There is a Gl Account with name similar to this";
                        ViewBag.Message = message;
                        return View(model);
                    }
                    else
                    {
                        GlAccount glAccount = new GlAccount();
                        glAccount.BranchId = model.GlAccount.BranchId;
                        glAccount.Name = model.GlAccount.Name;
                        glAccount.GlCategoryId = model.GlAccount.GlCategoryId;
                        var getGlCategory = glCatRepo.Get(glAccount.GlCategoryId);

                        glAccount.mainCategory = getGlCategory.mainAccountCategory.ToString();

                        glAccount.acountBalance = 0;
                        var FindGlCategory = _context.GlCategories.Where(a => a.id == glAccount.GlCategoryId).FirstOrDefault();

                        glAccount.mainCategory = FindGlCategory.mainAccountCategory.ToString();
                        if (FindGlCategory.name == "Cash")
                        {


                            glAccount.assignToTeller = "false";
                        }
                        else
                        {
                            glAccount.assignToTeller = "";
                        }
                        glAccount.Code = glAccountLogic.GenerateGlAccountCode(glAccount.GlCategoryId);
                        baserepo.Add(glAccount);
                        try
                        {

                            baserepo.Save(glAccount);
                            return RedirectToAction("Index", "GlAccount");
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

            }
            return RedirectToAction("Create","GlAccount");
    }

        //GET: GlAccount/Details/{id}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            GlAccount glAccount = baserepo.Get(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            return View(glAccount);
        }

        //GET: GlAccount/Edit/{id}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = _context.GlAccounts.Find(id);
            if (glAccount == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var viewModel = new GlAccountViewModels()
            //{
            //    Branches = _context.Branches.ToList(),
            //    GlCategories = _context.GlCategories.ToList()
            //};
            //return View(viewModel);
            GlAccountViewModels model = new GlAccountViewModels() { id = glAccount.id ,Name=glAccount.Name,BranchId=glAccount.BranchId, Branches = _context.Branches.ToList()};

            return View(model);
        }

        //POST: GlAccount/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,BranchId")] GlAccountViewModels model)
        {
            model.Branches = _context.Branches.ToList();
            if (model != null)
            {
                GlAccount glAccount = _context.GlAccounts.Find(model.id);
                glAccount.Name = model.Name;
                glAccount.BranchId = model.BranchId;
                
                try
                {
                    if (glAccountLogic.IsExist(glAccount)==false)
                    {
                        //Adding glAccount info to memory
                        _context.Entry(glAccount).State = EntityState.Modified;

                        //Updating glAccount info to the database
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        return RedirectToAction("Index", "GlAccount");
                    }
                    else {
                        ViewBag.Message = "Gl Account name already exist";
                        return View(model);
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
            return View(model);
        }
        
        //GET: GlAccount
        public ActionResult Index()
        {
            return View(_context.GlAccounts.ToList());
        }
    }
}
