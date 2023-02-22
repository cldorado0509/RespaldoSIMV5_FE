using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;

namespace SIM.Areas.Tramites
{
    public class TramitesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Tramites";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                name: "APIDefaultActionsTramites",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDefaultTramites",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Tramites_default",
                "Tramites/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Tramites.Controllers" }
            );
        }
    }
}