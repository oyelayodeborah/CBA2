using System;
using System.Collections.Generic;
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
    public class NodeController : Controller
    {
        BaseRepository<Node> nodeRepo = new BaseRepository<Node>(new ApplicationDbContext());
        NodeRepository Repo = new NodeRepository();
        // GET: Node
        public ActionResult Index()
        {
            var nodes = nodeRepo.GetAll();
            return View(nodes);
        }

        //Get
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Node model)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    //check uniqueness of name and code
                    if (!(Repo.isUniqueName(model.Name)))
                    {
                        ViewBag.Msg = "Node's name must be unique";
                        return View();
                    }
                    model.HostName = model.IPAddress;
                    model.Status = Status.Active;
                    nodeRepo.Save(model);
                    return RedirectToAction("Index", new { message = "Successfully added Node!" });
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                    return View(new { message = "Node was not successful" });
                }
            //}
            //return View(model);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Msg = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Node model = nodeRepo.Get((int)id);// = db.Customers.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Node model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    var node = nodeRepo.Get(model.Id);
               
                    //check uniqueness of name and code
                    if (!(Repo.isUniqueName(node.Name, model.Name)))
                    {
                        ViewBag.Msg = "Node's name must be unique";
                        return View();
                    }
                    node.HostName = model.IPAddress;
                    node.IPAddress = model.IPAddress;
                    node.Name = model.Name;
                    node.Port = model.Port;
                    nodeRepo.Update(node);
                    ViewBag.Msg = "Updated";
                    return RedirectToAction("Index", new { message = "Node was successfully updated" });
                }
            //    ViewBag.Msg = "Please enter correct data";
            //    return View();
            //}
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message="Error updating node" });
            }
        }
    }
}
   