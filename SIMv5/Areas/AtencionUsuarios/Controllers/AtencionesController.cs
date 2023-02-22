namespace SIM.Areas.AtencionUsuarios.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Web.Mvc;

    [Authorize]
    public class AtencionesController : Controller
    {
        public ActionResult Atenciones()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            ViewBag.CodFuncionario = codFuncionario;
            return View();
        }

    }
}