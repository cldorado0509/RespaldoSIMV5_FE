namespace SIM.Areas.Retributivas
{
    using System.Web.Http;
    using System.Web.Mvc;
    
    public class RetributivasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Retributivas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
            name: "APIRetributivasActions",
            routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional, action = "Get" }
            );

            context.Routes.MapHttpRoute(
              name: "Api_Post",
              routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional, action = "Post" }
            );

            context.Routes.MapHttpRoute(
              name: "Api_Put",
              routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional, action = "Put" }
            );

            //context.Routes.MapHttpRoute(
            //    name: "APIRetributivasActionsPost",
            //    routeTemplate: this.AreaName + "/api/{controller}/{action}/{values}",
            //    defaults: new { values = UrlParameter.Optional, action = "Post" }
            //    );

            context.Routes.MapHttpRoute(
                name: "APIRetributivas",
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