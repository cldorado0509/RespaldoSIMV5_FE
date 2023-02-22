using System.Web.Mvc;

namespace SIM.Areas.LimiteUrbano
{
    public class LimiteUrbanoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LimiteUrbano";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LimiteUrbano_default",
                "LimiteUrbano/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}