using System.Web.Mvc;

namespace SIM.Areas.Sau
{
    public class SauAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Sau";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Sau_default",
                "Sau/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Sau.Controllers" }
            );
        }
    }
}