using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class SinkNodeController:Controller
    {
        SinkNodeRepository Repo = new SinkNodeRepository();


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SinkNode model)
        {
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueName(model.Name)))
                {
                    ViewBag.Message = "Sink Node's name must be unique";
                    return View();
                }
                model.HostName = model.IPAddress;
                model.Status = Status.Active;
                model.NodeType = NodeType.Client;
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Sink Node!" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Sink Node was not successful" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            SinkNode channels = _context.SinkNodes.Find(id);
            if (channels == null)
            {
                return HttpNotFound();
            }

            return View(channels);


        }

        [HttpPost]
        public ActionResult Edit(SinkNode model)
        {
            var _context = new ApplicationDbContext();
            try
            {
                //if (ModelState.IsValid)
                //{
                var node = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueName(node.Name, model.Name)))
                {
                    ViewBag.Message = "Sink Node's name must be unique";
                    return View();
                }
                node.HostName = model.IPAddress;
                node.IPAddress = model.IPAddress;
                node.Name = model.Name;
                node.Port = model.Port;
                node.DateModified = DateTime.Now;
                Repo.Update(node);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "Sink Node was successfully updated" });
            }
            //    ViewBag.Message = "Please enter correct data";
            //    return View();
            //}
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating node" });
            }
        }

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList == null ? new List<SinkNode>() : getList);
        }
    }
}