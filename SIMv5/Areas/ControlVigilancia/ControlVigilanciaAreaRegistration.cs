using System.Web.Mvc;
using System.Web.Http;

namespace SIM.Areas.ControlVigilancia
{
    public class ControlVigilanciaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ControlVigilancia";
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
           name: "DefaultApi",
           routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}/{sf}/{cp}",
           defaults: new { id = RouteParameter.Optional, sf = RouteParameter.Optional, cp = RouteParameter.Optional }
            );
          

        
            context.MapRoute(
                "ControlVigilancia_default",
                "ControlVigilancia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.ControlVigilancia.Controllers" }
            );
        }
    }
}