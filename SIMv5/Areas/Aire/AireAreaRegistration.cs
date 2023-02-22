using System.Web.Mvc;
using System.Web.Http;

namespace SIM.Areas.Aire
{
    public class AireAreaRegistrationmalo : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Aire";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
           //context.Routes.MapHttpRoute(
           //name: "ControlVigilancia_api_default",
           //routeTemplate: this.AreaName + "/api/{controller}/{id}",
           //defaults: new { id = RouteParameter.Optional }
           // );

            context.Routes.MapHttpRoute(
           name: "AireApi",
           routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}/{sf}/{cp}",
           defaults: new { id = RouteParameter.Optional, sf = RouteParameter.Optional, cp = RouteParameter.Optional }
            );
          

        
            context.MapRoute(
                "Aire_default",
                "visita/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Aire.Controllers" }
            );
        }
    }
}