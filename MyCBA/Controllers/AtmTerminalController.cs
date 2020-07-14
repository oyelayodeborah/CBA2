using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;

namespace MyCBA.Controllers
{
    //[SessionRestrictLogic]
    public class AtmTerminalController : Controller
    {
        BaseRepository<AtmTerminal> AtmTerminalRepo = new BaseRepository<AtmTerminal>(new ApplicationDbContext());

        // GET: AtmTerminal
        AtmTerminalRepository Repo = new AtmTerminalRepository();
        // GET: AtmTerminal
        public ActionResult Index()
        {
            var terminals = AtmTerminalRepo.GetAll();
            return View(terminals);
        }

        //Get
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AtmTerminal model)
        {
                try
                {
                    model.Code = "00000000";
                    //check uniqueness of name and code
                    if (!(Repo.isUniqueCode(model.Code) && Repo.isUniqueName(model.Name)))
                    {
                        ViewBag.Msg = "Terminal name and code must be unique";
                        return View();
                    }
                    AtmTerminalRepo.Save(model);
                var Id = Convert.ToString(Repo.GetName(model.Name).Id);
                switch (Id.Count())
                {
                    case 1:
                        model.Code = "1000000" + Id;
                        break;
                    case 2:
                        model.Code = "100000" + Id;
                        break;
                    case 3:
                        model.Code = "10000" + Id;
                        break;
                    case 4:
                        model.Code = "1000" + Id;
                        break;
                    case 5:
                        model.Code = "100" + Id;
                        break;
                    case 6:
                        model.Code = "10" + Id;
                        break;
                    case 7:
                        model.Code = "1" + Id;
                        break;
                }
                AtmTerminalRepo.Update(model);

                return RedirectToAction("Index", new { message = "Successfully added Terminal!" });
                }
                catch (Exception ex)
                {
                    //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                    return View(new { message = ex });
                }
            
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Msg = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtmTerminal model = AtmTerminalRepo.Get((int)id);// = db.Customers.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AtmTerminal model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    var terminal = AtmTerminalRepo.Get(model.Id);
                
                //check uniqueness of name and code
                    var unique = Repo.isUniqueName(terminal.Name, model.Name);
                    if (!(/*Repo.isUniqueCode(terminal.Code, model.Code) &&*/ unique))
                    {
                        ViewBag.Msg = "Terminal name must be unique";
                        return View();
                    }
                    terminal.Name = model.Name;
                    terminal.Location = model.Location;
                    AtmTerminalRepo.Update(terminal);
                    ViewBag.Msg = "Updated";
                    return RedirectToAction("Index",new { message = "Terminal was successfully updated" });
                }
            //    ViewBag.Msg = "Please enter correct data";
            //    return View();
            //}
            //catch (Exception ex)
            //{
            //    //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
            //    return View(new { message = "Error updating terminal" });
            //}
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