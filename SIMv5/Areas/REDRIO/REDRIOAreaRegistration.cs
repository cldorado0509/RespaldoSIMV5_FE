namespace SIM.Areas.REDRIO
{
    using System.Web.Http;
    using System.Web.Mvc;

    public class REDRIOAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "REDRIO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
{
    
    context.Routes.MapHttpRoute(
        name: "APIRedrioActions",
        routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
        defaults: new { id = RouteParameter.Optional, action = "Get" }
    );

    context.Routes.MapHttpRoute(
    name: "APIRedrio",
    routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
    defaults: new { action = "Get", id = RouteParameter.Optional }
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
