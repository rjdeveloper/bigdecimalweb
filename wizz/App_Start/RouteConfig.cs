using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace wizz
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{area}/{controller}/{action}/{id}",
                defaults: new { area = "Admin", controller = "Home", action = "Login", id = UrlParameter.Optional },
                 namespaces: new string[] { "wizz.Areas.Admin.Controllers" 
                 }
            ).DataTokens.Add("area", "Admin");
        }
    }
}
