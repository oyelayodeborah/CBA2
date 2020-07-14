using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    public class GlCategoryController: Controller
    {
        BaseRepository<GlCategory> baserepo = new BaseRepository<GlCategory>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        GlCategoryLogic glCategoryLogic = new GlCategoryLogic();

        
        //GET: GlCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: GlCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GlCategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                //if (model != null && model.id == 0 && model.mainAccountCategory!=0)
                //{
                GlCategory glCategory = new GlCategory();
                glCategory.description = model.GlCategories.description;
                glCategory.mainAccountCategory = model.GlCategories.mainAccountCategory;
                glCategory.name = model.GlCategories.name;
                //checking if the glcategory name already exist
                if (glCategoryLogic.IsDetailsExist(glCategory.name)==true)
                {
                        //Assigning the values gotten from the create form to the GlCategory model
                        //GlCategory glCategory = new GlCategory();
                        glCategory.code = glCategoryLogic.GenerateCode(glCategory.mainAccountCategory);

                        try
                        {
                            //Adding glCategory info to memory
                            _context.GlCategories.Add(glCategory);

                            //Saving glCategory info to the database
                            _context.SaveChanges();
                            TempData["Message"] = "Success";
                            return RedirectToAction("Index", "GlCategory");
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
                        ViewBag.Message = "There is a Gl Category name similar to this";
                        return View(model);
                    }
                
            }
                else
                {
                return View(model);
                //return RedirectToAction("Create"); /*new { message="Create not successful" });*/
                //return View("Create", "GlCategory", model);
            }
            
            //else
            //{
            //    ViewBag.Message = "Kindly fill all the fields";
            //    return RedirectToAction("Create"); /*new { message="Create not successful" });*/
            //    //return View("Create", "GlCategory", model);
            //}

            
        }

        //GET: GlCategory/Details/{id}

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            GlCategory glcategories = baserepo.Get(id);
            if (glcategories == null)
            {
                return HttpNotFound();
            }
            return View(glcategories);
        }

        // GET: GlCategory/Edit/{id}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            GlCategory glCategory = _context.GlCategories.Find(id);
            if (glCategory == null)
            {
                return HttpNotFound();
            }

            GlCategoryViewModels model = new GlCategoryViewModels() { id = glCategory.id, name= glCategory.name, description = glCategory.description  };
            
            return View(model);


        }

        //Post: GlCategory/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description")] GlCategoryViewModels model)
        {
            GlCategory glCategory = _context.GlCategories.Find(model.id);

            glCategory.description = model.description;
            glCategory.name = model.name;

            try
            {
                if (glCategoryLogic.IsExist(glCategory)==true)
                {
                    //Adding glCategory info to memory
                    _context.Entry(glCategory).State = EntityState.Modified;

                    //Updating glCategory info to the database
                    _context.SaveChanges();
                    TempData["Message"] = "Success";

                    return RedirectToAction("Index", "GlCategory");
                }
                else {
                    ViewBag.Message = "There is a Gl Category with name similar to this";
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


        // GET: GlCategory
        public ActionResult Index()
        {
            //var glcategory = baserepo.GetAll();
            //return View(glcategory.ToList());
            return View(_context.GlCategories.ToList());
        }
    }
}
