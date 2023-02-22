namespace SIM.Areas.EtiquetasAmbientales
{
    using System.Web.Http;
    using System.Web.Mvc;

    public class ExpedienteAmbientaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EtiquetasAmbientales";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                  name: "APIEtiquetasAmbientalesActions",
                  routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                  defaults: new { id = RouteParameter.Optional, action = "Get" }
              );

            context.Routes.MapHttpRoute(
                name: "APIEtiquetasAmbientales",
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