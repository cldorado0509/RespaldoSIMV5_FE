using System.Web.Mvc;
using System.Web.Http;

namespace SIM.Areas.Documento
{
    public class DocumentoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Documento";
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
           name: "DocumentoApi",
           routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}/{sf}/{cp}",
           defaults: new { id = RouteParameter.Optional, sf = RouteParameter.Optional, cp = RouteParameter.Optional }
            );
          

        
            context.MapRoute(
                "Documento_default",
                "Documento/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Documento.Controllers" }
            );
        }
    }
}