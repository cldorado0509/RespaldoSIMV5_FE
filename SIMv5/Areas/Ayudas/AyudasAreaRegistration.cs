using System.Web.Mvc;
using System.Web.Http;

namespace SIM.Areas.Ayudas
{
    public class AyudasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ayudas";
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
           name: "AyudasApi",
           routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}/{sf}/{cp}",
           defaults: new { id = RouteParameter.Optional, sf = RouteParameter.Optional, cp = RouteParameter.Optional }
            );
          

        
            context.MapRoute(
                "Ayudas_default",
                "Ayudas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Ayudas.Controllers" }
            );
        }
    }
}