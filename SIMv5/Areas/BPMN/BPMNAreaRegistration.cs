using System.Web.Http;
using System.Web.Mvc;

namespace SIM.Areas.BPMN
{
    public class BPMNAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BPMN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
           
            context.Routes.MapHttpRoute(
               name: "APIDefaultActionsBPMN",
               routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional, action = "Get" }
           );

            context.Routes.MapHttpRoute(
                name: "APIDefaultBPMN",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "BPMN_default",
                "BPMN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.BPMN.Controllers" }
            );





        }
    }
}