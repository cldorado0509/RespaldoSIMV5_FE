using System.Web.Mvc;

namespace SIM.Areas.Riesgos
{
    public class RiesgosRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Riesgos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Quimicos_default",
                "Quimicos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Riesgos.Controllers" }
            );
        }
    }
}