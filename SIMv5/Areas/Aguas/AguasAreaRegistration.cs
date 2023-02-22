using System.Web.Mvc;

namespace SIM.Areas.Aguas
{
    public class AguasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Aguas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Aguas_default",
                "Aguas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Aguas.Controllers" }
            );
        }
    }
}