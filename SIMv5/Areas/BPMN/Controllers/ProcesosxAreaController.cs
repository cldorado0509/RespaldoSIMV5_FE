using SIM.Areas.ControlVigilancia.Models;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.BPMN.Controllers
{
    [Authorize]
    public class ProcesosxAreaController : Controller
    {
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        // GET: BPMN/MaestroProcesos
        public ActionResult Index()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            ViewBag.CodFuncionario = codFuncionario;
            return View();
        }
    }
}