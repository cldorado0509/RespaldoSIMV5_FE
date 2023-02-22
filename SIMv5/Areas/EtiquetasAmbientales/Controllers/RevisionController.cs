using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.EtiquetasAmbientales.Controllers
{
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Data;
    using SIM.Models;
    using System;
    using System.Security.Claims;
    using System.Web.Mvc;

    public class RevisionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        // GET: AdminDocumental/RevisionDatos
        public ActionResult RevisionDatos()
        {
            //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //decimal codFuncionario = -1;
            //if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            //{
            //    int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            //    codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            //}

            //ViewBag.CodFuncionario = codFuncionario;
            return View();
        }

    }
}