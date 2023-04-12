namespace SIM.Areas.AdministracionDocumental.Controllers
{
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Data;
    using SIM.Models;
    using System;
    using System.Security.Claims;
    using System.Web.Mvc;

    public class AdminDocumentalController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        // GET: AdminDocumental/AdminDocumental
        public ActionResult ClasificacionDoc()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }

            ViewBag.CodFuncionario = codFuncionario;
            return View();
        }

        // GET: AdminDocumental/TablasRetencion
        public ActionResult TablasRetencion()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }

            ViewBag.CodFuncionario = codFuncionario;
            return View();
        }

    }
}