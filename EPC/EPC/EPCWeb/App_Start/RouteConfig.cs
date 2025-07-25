using System.Web.Mvc;
using System.Web.Routing;

namespace EPCWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            _ = routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EPCModule", action = "EPCModule", id = UrlParameter.Optional }
            );
        }
    }
}
