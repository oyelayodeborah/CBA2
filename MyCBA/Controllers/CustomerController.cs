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
    [AdminRoleRestrictLogic]
    public class CustomerController : Controller
    {
        BaseRepository<Customer> baserepo = new BaseRepository<Customer>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        CustomerLogic customerLogic = new CustomerLogic();
        CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        // GET: Customer
        public ActionResult Index()
        {
            //var customers = baserepo.GetAll();
            //return View(customers.ToList());
            return View(_context.Customers.ToList());
        }

        //GET: Customer/Create

        public ActionResult Create()
        {
            TempData["Message"] = "";

            return View();
        }

        //POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,address,email,phoneNumber,gender")] Customer customer)
        {
            TempData["Message"] = "";

            if (ModelState.IsValid)
            {
                try
                {
                    if (customerLogic.IsDetailsExist(customer.email,customer.phoneNumber))
                    {
                        baserepo.Save(customer);

                        var value = "00000";
                        var custid = value + customer.id;
                        customer.customerID = custid;
                        _context.Entry(customer).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["Message"] = "Success";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Exist";
                        return View(customer);

                    }                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                    return View(customer);
                }

            }
            return View();
        }

        [AdminRoleRestrictLogic]
        //GET: Customer/OpenAccount
        public ActionResult OpenAccount(int id)
        {
            var viewModel = new CustomerAccountViewModels()
            {
                id=id,
                Branches = _context.Branches.ToList(),
            };
            return View(viewModel);
        }

        //POST: Customer/OpenAccount
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenAccount([Bind(Include = "id,acctName,customerAccount,accType")] CustomerAccountViewModels model)
        {
            model.Branches = _context.Branches.ToList();
            

            if (model != null && model.accType != 0  && model.customerAccount.branchId != 0)
            {
                model.Customers = _context.Customers.ToList();
                Customer cust = _context.Customers.Find(model.id);
                if (cust == null)
                {
                    ViewBag.Message = "Exist";
                    return View();
                }
                //Assigning the values gotten from the create form to the CustomerAccount model
                CustomerAccount customerAccount = new CustomerAccount();
                customerAccount.customerId = cust.id;
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

        //GET: Customer/Details/{id}

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Customer customer = baserepo.Get(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //GET: Customer/Edit/{id}

        public ActionResult Edit(int? id)
        {
            TempData["Message"] = "";

            if (id == null)
            {
                return HttpNotFound();
            }
            Customer customer = baserepo.Get(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //POST: Customer/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,address,email,phoneNumber,gender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    if (!customerLogic.IsEditDetailsExist(customer.email, customer.phoneNumber))
                    {
                        _context.Entry(customer).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        return RedirectToAction("Index");
                    }
                    TempData["Message"] = "Exist";

                    return View(customer);

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.ToString());
                    return View(customer);
                }
            }
            return View();
        }

    }

}
