using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.CAV.Controllers
{
    /// <summary>
    /// Registro CAV Controller
    /// </summary>
    public class RegistroCAVController : Controller
    {

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Registro CAV Controller
        /// </summary>
        /// <returns></returns>
        // GET: CAV/RegistroCAV
        public ActionResult Index()
        {
            var controller = RouteData.Values["controller"].ToString();
            var area = RouteData.DataTokens["area"].ToString();
            var action = RouteData.Values["action"].ToString();

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;

            var idusuario = (User.Identity as ClaimsIdentity).Claims.Where(f => f.Type.EndsWith("nameidentifier")).FirstOrDefault();
            int idUsuario = 0;
            int.TryParse(idusuario.Value, out idUsuario);

            codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                              join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                              where uf.ID_USUARIO == idUsuario
                                              select f.CODFUNCIONARIO).FirstOrDefault());

            ViewBag.CodFuncionario = codFuncionario;
            ViewBag.CodigoUnidadDocumental = SIM.Utilidades.Data.ObtenerValorParametro("IdCodSerieHistoriasAmbientales").ToString();

            var idForma = 0;
            int.TryParse(SIM.Utilidades.Data.ObtenerValorParametro("IdFormaExpedientesAmbientales").ToString(), out idForma);

            PermisosRolModel permisosRolModel = SIM.Utilidades.Security.PermisosFormulario(area, controller, action, idUsuario);

            ViewBag.CodFuncionario = codFuncionario;
            return View(permisosRolModel);

        }
    }
}