using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using MyCBA.Core.Models;

namespace MyCBA
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Database.SetInitializer<ApplicationDbContext>(null);

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
