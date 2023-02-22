using System.Web.Mvc;
using System.Web.Http;

namespace SIM.Areas.EncuestaExterna
{
    public class TalaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EncuestaExterna";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
       
            context.Routes.MapHttpRoute(
           name: "EncuestaExternaApi",
           routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}/{sf}/{cp}",
           defaults: new { id = RouteParameter.Optional, sf = RouteParameter.Optional, cp = RouteParameter.Optional }
            );
          

        
            context.MapRoute(
                "EncuestaExterna_default",
                "EncuestaExterna/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.EncuestaExterna.Controllers" }
            );
        }
    }
}