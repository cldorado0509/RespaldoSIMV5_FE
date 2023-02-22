namespace SIM.Areas.MiBici
{
    using System.Web.Mvc;
    using System.Web.Http;


    public class MiBiciAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MiBici";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
              name: "APIDefaultActionsMiBici",
              routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional, action = "Get" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDefaultMiBici",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "MiBici_default",
                "MiBici/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.MiBici.Controllers" }
            );
        }
    }
}