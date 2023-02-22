using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Areas.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Contratos.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;

        // GET: Contratos/Procesos
        public ActionResult Index()
        {
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