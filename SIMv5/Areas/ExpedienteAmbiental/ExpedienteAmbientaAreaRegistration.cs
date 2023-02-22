namespace SIM.Areas.EtiquetasAmbientales
{
    using System.Web.Http;
    using System.Web.Mvc;

    public class ExpedienteAmbientalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ExpedienteAmbiental";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                  name: "APIExpedienteAmbientalActions",
                  routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                  defaults: new { id = RouteParameter.Optional, action = "Get" }
              );

            context.Routes.MapHttpRoute(
                name: "APIExpedienteAmbiental",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                this.AreaName + "_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}