namespace SIM.Areas.ParqueAguas
{
    using System.Web.Http;
    using System.Web.Mvc;
    public class ParqueAguasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ParqueAguas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                 name: "APIParqueAguasActions",
                 routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional, action = "Get" }
             );

            context.Routes.MapHttpRoute(
                name: "APIParqueAguas",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "ParqueAguas_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}