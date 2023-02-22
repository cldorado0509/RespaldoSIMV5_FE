using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;

namespace SIM.Areas.AtencionUsuarios
{
    public class QuejasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AtencionUsuarios";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            

            context.Routes.MapHttpRoute(
                name: "APIDefaultActionsMV",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }
               
            );

            context.MapRoute(
                "AtencionUsuarios_Default",
                "AtencionUsuarios/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.AtencionUsuarios.Controllers" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDefaultMV",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}