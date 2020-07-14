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
    public class RoleController : Controller
    {
        RoleLogic roleLogic = new RoleLogic();
        RoleRepository roleRepo = new RoleRepository();
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Role
        public ActionResult Index()
        {
            return View(_context.Roles.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        //Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name")]  Role model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (roleLogic.IsDetailsExist(model.name))
                    {
                        roleRepo.Save(model);
                        TempData["Message"] = "Success";
                        //var getRole = roleRepo.GetAll().Count();
                        //if (getRole == 1)
                        //{
                        //    return RedirectToAction("Register", "Account");

                        //}
                        //else
                        //{
                        //    return RedirectToAction("Index");
                        //}
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
            Role role = roleRepo.Get(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //POST: Role/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Role role)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (!roleLogic.IsEditDetailsExist(role.name))
                    {
                        _context.Entry(role).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        return RedirectToAction("Index");
                    }
                    ViewBag.Message = "Exist";

                    return View(role);

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.ToString());
                    return View(role);
                }
            }
            return View();
        }

    }

}