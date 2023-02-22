using System.Web.Mvc;

namespace SIM.Areas.TalaPoda
{
    public class TalaPodaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TalaPoda";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TalaPoda_default",
                "TalaPoda/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.TalaPoda.Controllers" }
            );
        }
    }
}