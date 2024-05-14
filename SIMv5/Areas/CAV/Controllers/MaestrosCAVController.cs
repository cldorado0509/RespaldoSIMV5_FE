using SIM.Areas.Seguridad.Class;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;


namespace SIM.Areas.CAV.Controllers
{
    /// <summary>
    /// /Maestros CAV Controller
    /// </summary>
    public class MaestrosCAVController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Maestros CAV Controller
        /// </summary>
        /// <returns></returns>
        public ActionResult TiposAdquisicion()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            int idUsuario = 0;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                              join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                              where uf.ID_USUARIO == idUsuario
                                              select f.CODFUNCIONARIO).FirstOrDefault());

            ViewBag.CodFuncionario = codFuncionario;
            ViewBag.CodigoUnidadDocumental = SIM.Utilidades.Data.ObtenerValorParametro("IdCodSerieHistoriasAmbientales").ToString();

            var idForma = 0;
            int.TryParse(SIM.Utilidades.Data.ObtenerValorParametro("IdFormaExpedientesAmbientales").ToString(), out idForma);


            PermisosRolModel permisosRolModel = new PermisosRolModel { CanDelete=false, CanInsert=false, CanPrint=false, CanRead= false, CanUpdate = false, IdRol = 0 };

            Permisos permisos = new Permisos();
            permisosRolModel = permisos.ObtenerPermisosRolForma(idForma, idUsuario);

            ViewBag.CodFuncionario = codFuncionario;
            return View(permisosRolModel);
        }



    }
}