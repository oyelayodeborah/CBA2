using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Logic;

namespace MyCBA.Controllers
{
    [AdminRoleRestrictLogic]
    public class EODController:Controller
    {
        public ActionResult Index()
        {
            EODLogic busConfigLogic = new EODLogic();
             var output=busConfigLogic.RunEOD();
            TempData["message"] = output;
            return View();
        }
        
        public ActionResult Setup()
        {
            SetupBusiness();
            TempData["Message"] = "Success";
            ViewBag.Message = "Success";
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }
        public ActionResult Open()
        {
            EODLogic busConfigLogic = new EODLogic();
            busConfigLogic.Open();
            TempData["Message"] = "Success";
            ViewBag.Message = "Success";
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }
        public ActionResult Close()
        {
            EODLogic busConfigLogic = new EODLogic();
            busConfigLogic.Close();
            busConfigLogic.RunEOD();
            TempData["Message"] = "Success";
            ViewBag.Message = "Success";
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }

        public ActionResult RunEOD()
        {
            EODLogic busConfigLogic = new EODLogic();
            var output= busConfigLogic.RunEOD();
            TempData["message"] = output;
            TempData["Message"] = "Success";
            ViewBag.Message = "Success";
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }
        public void SetupBusiness()
        {
           AccountConfigurationLogic acctConfigLogic = new AccountConfigurationLogic();

            var appDbContext = new ApplicationDbContext();
            BusinessConfig businessConfig = new BusinessConfig();
            businessConfig.FinancialDate = DateTime.Now;
            businessConfig.IsBusinessOpen = true;
            businessConfig.DayCount = 0;
            businessConfig.MonthCount = 0;
            businessConfig.YearCount = 0;
            appDbContext.BusinessConfigs.Add(businessConfig);
            appDbContext.SaveChanges();
            acctConfigLogic.SaveBusinessConfig(businessConfig);
        }

    }
}
