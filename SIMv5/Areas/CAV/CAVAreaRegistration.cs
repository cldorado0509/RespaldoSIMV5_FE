using System.Web.Http;
using System.Web.Mvc;

namespace SIM.Areas.CAV
{
    public class CAVAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CAV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                 name: "APICAVActions",
                 routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional, action = "Get" }
             );

            context.Routes.MapHttpRoute(
                name: "APICAV",
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