using System.Web.Mvc;

namespace SIM.Areas.Formulario
{
    public class FormularioRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Formularios";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Visitas_default",
                "Visitas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIM.Areas.Formularios.Controllers" }
            );
        }
    }
}