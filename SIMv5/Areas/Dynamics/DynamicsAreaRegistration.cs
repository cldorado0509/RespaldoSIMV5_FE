using System.Web.Http;
using System.Web.Mvc;

namespace SIM.Areas.Dynamics
{
    public class DynamicsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dynamics";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "ApiActionDynamics_default",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDynamics_default",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Dynamics_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
            );
        }


    }
}