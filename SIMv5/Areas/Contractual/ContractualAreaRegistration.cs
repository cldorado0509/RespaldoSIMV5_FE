using System.Web.Http;
using System.Web.Mvc;

namespace SIM.Areas.Contractual
{
    public class ContractualAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Contractual";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "APIDefaultActionsContractual",
                routeTemplate: this.AreaName + "/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Get" }
            );

            context.Routes.MapHttpRoute(
                name: "APIDefaultContractual",
                routeTemplate: this.AreaName + "/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Contractual_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}