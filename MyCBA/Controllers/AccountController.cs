using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;
using MyCBA.ViewModels;

namespace MyCBA.Controllers
{
    public class AccountController : Controller
    {
        BaseRepository<User> baserepo = new BaseRepository<User>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        UserLogic userLogic = new UserLogic();
        RoleRepository roleRepo = new RoleRepository();
        BranchRepository branchRepo = new BranchRepository();


        //GET: User/Create
        [AllowAnonymous]
        public ActionResult Register()
        {
            TempData["Message"] = null;
            UserRepository userRepo = new UserRepository();
            var user = userRepo.GetAll();
            var use = 0;
            if (user.Count()== 0)
            {
                var viewModel = new UserViewModels()
                {
                    Branches = _context.Branches.ToList(),
                    Roles = _context.Roles.ToList().Where(c => c.name == "Admin")

                };
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            
            
        }

        //POST: User/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModels userViewModels)
        {
            User user = new User();
            userViewModels.Branches = _context.Branches.ToList();
            userViewModels.Roles = _context.Roles.ToList().Where(c => c.name == "Admin");

            var passwordHash = userLogic.GeneratePassword();
            user.passwordHash = Crypto.Hash(passwordHash);
            user.username = userViewModels.username;
            user.phoneNumber = userViewModels.phoneNumber;
            user.LoggedIn = "";
            user.fullName = userViewModels.fullName;
            user.branchId = userViewModels.branchId;
            user.email = userViewModels.email;
            user.roleId = userViewModels.roleId;
            user.IsAssigned = "";
            _context.Users.Add(user);
            
            try     //if success
                {
                    var sendMail = userLogic.SendingEmail(userViewModels.email, passwordHash, userViewModels.fullName);
                    if (sendMail == "Successful")
                    {
                        _context.SaveChanges();
                        

                    ViewBag.Message = "Success";

                    return RedirectToAction("Login","Account");
                    }
                    else
                    {
                        TempData["Message"] = "Email error";
                        return View(userViewModels);
                    }

                }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                TempData["Message"] = "Error";
                return View(userViewModels);
            }

        }

        [AdminRoleRestrictLogic]
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
        [AdminRoleRestrictLogic]
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
                    if (Session["role"].ToString() == "")
                    {
                        int id = Convert.ToInt32(Session["id"].ToString());
                        findPassword = _context.Users.Where(a => a.passwordHash == current_password && a.id == id).FirstOrDefault();
                        if (findPassword == null)
                        {
                            ViewBag.Message = "Incorrect password";
                            return View();
                        }
                        User user = _context.Users.Find(findPassword.id);
                        user.passwordHash = Crypto.Hash(model.new_password);
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        ViewBag.Message= "Success";
                        return RedirectToAction("Login","Account");
                    }
                    else
                    {
                        findPassword = _context.Users.Where(a => a.passwordHash == current_password).FirstOrDefault();
                        if (findPassword == null)
                        {
                            ViewBag.Message = "Incorrect password";
                            return View();
                        }
                        User user = _context.Users.Find(findPassword.id);
                        user.passwordHash = Crypto.Hash(model.new_password);
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["Message"] = "Success";
                        ViewBag.Message = "Success";
                        return RedirectToAction("Login", "Account");
                    }

                }
            }
            ViewBag.Message = "An error occurred while verifying";
            return View(model);

        }

        // GET: Account/Login

        public ActionResult Login(int? id)
        {
            UserRepository userRepo = new UserRepository();
            var user = userRepo.GetAll();
            if (user.Count() == 0)
            {
                return RedirectToAction("Register", "Account");
            }
            else
            {
            return View();
            }
        }

        //POST: Account/Login
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Login()
        {
            User user = new User();
            user.email = Request.Form["email"];
            user.passwordHash = Request.Unvalidated["passwordHash"];

            ViewBag.Message = "";
            if (Session["id"] == null)
            {
                if (user == null)
                {
                    return View("Login");
                }
                var getEmail = (user.email).ToLower().Trim();
                var getPassword = user.passwordHash;
                var hashedPassword = Crypto.Hash(user.passwordHash.Trim());
                var findEmailAndPassword = _context.Users.Where(a => a.passwordHash == hashedPassword && a.email == getEmail).FirstOrDefault();
                if (findEmailAndPassword != null)
                {
                    Session["id"] = findEmailAndPassword.id;
                    Session["password"] = getPassword;
                    Session["username"] = findEmailAndPassword.username;
                    Session["name"] = findEmailAndPassword.fullName;
                    var getRole= roleRepo.Get(findEmailAndPassword.roleId); ;
                    Session["role"] = getRole.name;
                    Session["email"] = getEmail;
                    Session["currenttillbalance"] = "";
                    Session["tillbalance"] = "";
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Message = "Incorrect login details";
                return View(user);
            }
            ViewBag.Message = "Session currently Exist";
            return View();
            //return RedirectToAction("Logout", "Home");
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}