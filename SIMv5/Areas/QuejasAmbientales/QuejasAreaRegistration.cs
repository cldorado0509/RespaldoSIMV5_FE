using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;

namespace SIM.Areas.QuejasAmbientales
{
    public class QuejasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "QuejasAmbientales";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            

            context.Routes.MapHttpRoute(
                name: "APIDefaultActionsQuejasAmbientales",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }
               
            );

            context.MapRoute(
                "QuejasAmbientales",
                "QuejasAmbientales/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.QuejasAmbientales.Controllers" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDefaultQuejasAmbientales",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}