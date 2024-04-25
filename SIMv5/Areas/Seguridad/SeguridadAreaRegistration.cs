using System.Web.Http;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad
{
    public class SeguridadAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Seguridad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            /*context.MapRoute(
                "Seguridad_default",
                "Seguridad/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );*/

            context.Routes.MapHttpRoute(
                name: "APIDefaultActions" + this.AreaName,
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }/*,
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }*/
            );

            context.Routes.MapHttpRoute(
                name: "APIDefault" + this.AreaName,
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                this.AreaName + "Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}