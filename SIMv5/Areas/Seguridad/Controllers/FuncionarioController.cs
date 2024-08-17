using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FuncionarioController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Seguridad/Funcionario
        //[Authorize(Roles = "VFUNCIONARIO")]
        public ActionResult Index()
        {
            string actionName = RouteData.Values["action"].ToString();
            string controllerName = RouteData.Values["controller"].ToString();
            string areaName = RouteData.DataTokens["area"].ToString();
            var _IdUsuario = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("nameidentifier")).FirstOrDefault();
            decimal IdUsuario = 0;
            decimal.TryParse(_IdUsuario.Value, out IdUsuario);
            PermisosRolModel permisos = SIM.Utilidades.Security.PermisosFormulario(areaName, controllerName, actionName, IdUsuario);
            return View(permisos);
        }
    }
}