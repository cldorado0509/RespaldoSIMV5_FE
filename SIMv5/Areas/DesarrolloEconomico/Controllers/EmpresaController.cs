namespace SIM.Areas.DesarrolloEconomico.Controllers
{
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Areas.Models;
    using SIM.Data;
    using SIM.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    public class EmpresaController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        // GET: DesarrolloEconomico/Empresa
        [Authorize]
        public ActionResult Index()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }

            //ViewBag.CodFuncionario = 4005;// ModelsToListSeguridad.ObtenerIdFuncionario(User.Identity.GetUserId());
            ViewBag.CodFuncionario = codFuncionario;

            return View();
        }
    }
}