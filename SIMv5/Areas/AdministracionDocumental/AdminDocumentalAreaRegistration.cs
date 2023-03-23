namespace SIM.Areas.AdministracionDocumental
{
    using System.Web.Http;
    using System.Web.Mvc;

    public class AdministracionDocumentalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdministracionDocumental";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                 name: "APIAdminDocumentalActions",
                 routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional, action = "Get" }
             );

            context.Routes.MapHttpRoute(
                name: "APIAdminDocumental",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Admin_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}