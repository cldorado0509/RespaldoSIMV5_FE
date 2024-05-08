using System.Web.Http;
using System.Web.Mvc;

namespace SIM.Areas.ProcesosJudiciales
{
    public class ProcesosJudicialesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ProcesosJudiciales";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                 name: "APIDefaultActionsJudiciales",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional, action = "Get" }
           );

            context.Routes.MapHttpRoute(
                name: "APIDefaultJudiciales",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
               "ProcesosJudiciales_default",
               this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
           );
        }
    }
}