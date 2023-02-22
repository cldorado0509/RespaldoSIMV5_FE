namespace SIM.Areas.GestionRiesgo
{
    using System.Web.Mvc;
    using System.Web.Http;

    public class GestionRiesgoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GestionRiesgo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            
            context.Routes.MapHttpRoute(
                name: "APIDefaultActionsGR",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDefaultGR",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "GestionRiesgo_default",
                "GestionRiesgo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.GestionRiesgo.Controllers" }
            );
        }

    }
}