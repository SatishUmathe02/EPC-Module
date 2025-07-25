using System.Web.Http;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //cors.ExposedHeaders.Add("x-filename");
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            _ = config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
