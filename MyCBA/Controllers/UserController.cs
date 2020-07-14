using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.ViewModels;
using MyCBA.Logic;
using System.Data.Entity;

namespace MyCBA.Controllers
{
    [SessionRestrictLogic]
    public class UserController : Controller
    {
        BaseRepository<User> baserepo = new BaseRepository<User>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        UserLogic userLogic = new UserLogic();
        RoleRepository roleRepo = new RoleRepository();


        // GET: User/ChangePassword
        [SessionRestrictLogic]
        public ActionResult ChangePassword()
        {
            if (Session["id"] != null)
            {
                return View();
            }
            TempData["Message"] = "";
            return RedirectToAction("Login");
        }

        // POST: User/ChangePassword
        [SessionRestrictLogic]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangeUserPasswordViewModels model)
        {
            var val = Request.Form["session"];
            var sessionID = Convert.ToInt32(val);

            if (model != null)
            {
                ViewBag.Message = "";

                var current_password = Crypto.Hash(model.current_password.Trim());
                var findPassword = new User();
                if (sessionID == 0)
                {
                    HttpNotFound();
                }
                else
                {
                    if (Session["role"].ToString() == "Teller")
                    {
                        int id = Convert.ToInt32(Session["id"].ToString());
                        findPassword = _context.Users.Where(a => a.passwordHash == current_password && a.id == id).FirstOrDefault();
                        if (findPassword != null)
                        {
                            User user = _context.Users.Find(findPassword.id);
                            user.passwordHash = Crypto.Hash(model.new_password);
                            _context.Entry(user).State = EntityState.Modified;
                            _context.SaveChanges();
                            ViewBag.Message = "Password was successfully updated";
                            return RedirectToAction("Logout", "Home");
                        }
                        else
                        {
                            ViewBag.Invalid = "Invalid Current Password";
                            return View(model);
                        }

                    }
                    else
                    {
                        findPassword = _context.Users.Where(a => a.passwordHash == current_password && a.username == model.username).FirstOrDefault();
                        if (findPassword != null)
                        {
                            User user = _context.Users.Find(findPassword.id);
                            user.passwordHash = Crypto.Hash(model.new_password);
                            _context.Entry(user).State = EntityState.Modified;
                            _context.SaveChanges();
                            ViewBag.Message = "Password was successfully updated";
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            ViewBag.Invalid = "Invalid Current Password";
                            return View(model);
                        }
                    }

                }
            }
            ViewBag.Message = "An error occurred while verifying";
            return View(model);

        }

        //GET: User/Create
        [AdminRoleRestrictLogic]
        public ActionResult Create()
        {
            TempData["Message"] = "";

            var viewModel = new UserViewModels()
            {
                Branches = _context.Branches.ToList(),
                Roles = _context.Roles.ToList()

            };
            return View(viewModel);
        }

        //POST: User/Create
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(UserViewModels userViewModels)
        public ActionResult Create([Bind(Include = "fullName,email,phoneNumber,roleId,username,branchId")] UserViewModels userViewModels)
        {
            User user = new User();
            userViewModels.Branches = _context.Branches.ToList();
            userViewModels.Roles = _context.Roles.ToList();
            TempData["Message"] = "";

            if (ModelState.IsValid)
            {
              //checking if the unique values already exist in the database
              var itExist = userLogic.IsDetailsExist(userViewModels.email, userViewModels.phoneNumber, userViewModels.username);
                if (itExist)            //if true
                {
                    TempData["Message"] = "Exist";
                    return View(userViewModels);
                }
                else                    //else create the user
                {

                    var passwordHash = userLogic.GeneratePassword();        //generating password
                    user.passwordHash = Crypto.Hash(passwordHash);          //password hashing
                    user.username = userViewModels.username;
                    user.fullName = userViewModels.fullName;
                    user.email = userViewModels.email;
                    user.LoggedIn = "";
                    user.phoneNumber = userViewModels.phoneNumber;
                    user.branchId = userViewModels.branchId;
                    user.roleId= userViewModels.roleId;

                    var role = roleRepo.Get(userViewModels.roleId);
                    var roleName = role.name;
                    if (roleName == "Teller")          //if user is a teller, so that he can be assigned till account later on
                    {
                        user.IsAssigned = "false";
                    }
                    else
                    {
                        user.IsAssigned = "";               //else user is an Admin and no till account can be assigned
                    }
                    _context.Users.Add(user);

                    try     //if success
                    {
                        var sendMail = userLogic.SendingEmail(userViewModels.email, passwordHash, userViewModels.fullName);
                        if (sendMail == "Successful")
                        {
                            _context.SaveChanges();

                            TempData["Message"] = "Success";

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "Email error";
                            return View(userViewModels);
                        }
                        
                    }
                    catch (Exception ex)        //if failed
                    {
                        ModelState.AddModelError("", ex.ToString());
                        TempData["Message"] = "Error";
                        return View(userViewModels);
                    }
                }

            }
            TempData["Message"] = "Error";
            return View(userViewModels);

        }

        //GET: User/Details/{id}
        [AdminRoleRestrictLogic]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            User user = baserepo.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Edit/{id}
        [AdminRoleRestrictLogic]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            User user = _context.Users.Find(id);
            TempData["Message"] = "";
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModels model = new UserViewModels() { id = user.id, branchId=user.branchId,fullName=user.fullName,email=user.email,
                phoneNumber=user.phoneNumber, Branches = _context.Branches.ToList(),
            };

            TempData["Message"] = "";
            return View(model);
        }

        //POST: User/Edit/{id}
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fullName,email,phoneNumber,branchId")] UserViewModels model)
        {
            User user = _context.Users.Find(model.id);
            model.Branches = _context.Branches.ToList();
            TempData["Message"] = "";
            model.roleId = user.roleId;
            model.username = user.username;
            model.Roles = _context.Roles.ToList();

            //if (ModelState.IsValid)
            //{
                var itExist = userLogic.IsEditDetailsExist(model.email, model.phoneNumber);
                if (itExist)            //if true
                {
                    TempData["Message"] = "Exist";
                    return View(model);
                }
                else                    //else create the user
                {
                    user.fullName = model.fullName;
                    user.email = model.email;
                    user.phoneNumber = model.phoneNumber;
                    user.branchId = model.branchId;

                    _context.Entry(user).State = EntityState.Modified;

                    try     //if success
                    {
                        _context.SaveChanges();
                        TempData["Message"] = "Success";

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)        //if failed
                    {
                        ModelState.AddModelError("", ex.ToString());
                        TempData["Message"] = "Error";
                        return View(model);
                    }
                }

            //}
            //else {
            //    TempData["Message"] = "Error";
            //    return View(model);
            //}
           

        }

        // GET: User
        [AdminRoleRestrictLogic]
        public ActionResult Index()
        {
            return View(_context.Users.ToList());
        }


    }
}