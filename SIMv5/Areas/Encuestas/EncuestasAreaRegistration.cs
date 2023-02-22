using System.Web.Mvc;

namespace SIM.Areas.Encuestas
{
    public class EncuestasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Encuestas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Encuestas_default",
                "Encuestas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Encuestas.Controllers" }
            );
        }
    }
}