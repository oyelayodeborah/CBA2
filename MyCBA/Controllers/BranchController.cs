using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;

namespace MyCBA.Controllers
{
    [AdminRoleRestrictLogic]
    public class BranchController : Controller
    {
        BranchLogic branchLogic = new BranchLogic();
        BranchRepository branchRepo = new BranchRepository();
        ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Branch
        public ActionResult Index()
        {
            return View(_context.Branches.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        //POST:Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name")]  Branch model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (branchLogic.IsDetailsExist(model.name))
                    {
                        branchRepo.Save(model);
                        TempData["Message"] = "Success";
                        
                            return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Message = "Exist";
                        return View(model);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                    return View(model);
                }
            }
            return View(model);
        }

        //GET: Role/Edit/{id}

        public ActionResult Edit(int? id)
        {
            TempData["Message"] = "";

            if (id == null)
            {
                return HttpNotFound();
            }
            Branch branch = branchRepo.Get(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        //POST: Role/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Branch branch)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (!branchLogic.IsEditDetailsExist(branch.name))
                    {
                        _context.Entry(branch).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        return RedirectToAction("Index");
                    }
                    ViewBag.Message = "Exist";

                    return View(branch);

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.ToString());
                    return View(branch);
                }
            }
            return View();
        }

    }

}