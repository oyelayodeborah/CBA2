using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyCBA.Logic
{
    public class AdminRoleRestrictLogic : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var MySession = HttpContext.Current.Session;
            var sessionToString = (string)MySession["role"];
            if (sessionToString == null || sessionToString == "" || sessionToString != "Admin")
            {
                filterContext.Result = new RedirectResult(string.Format("~/Home/Logout/"));
            }
        }
    }
}
    
