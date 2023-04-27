namespace SIM.Areas.ControlVigilancia.Controllers
{
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Areas.Seguridad.Class;
    using SIM.Areas.Seguridad.Models;
    using SIM.Data;
    using SIM.Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    public class ReposicionesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        // GET: ControlVigilancia/Reposiciones
        public ActionResult Reposiciones()
        {

            PermisosRolModel permisosRolModel = new PermisosRolModel { CanDelete=false,CanInsert=false, CanPrint=false, CanRead= false, CanUpdate = false, IdRol = 0 };

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);

                var idForma = 0;
                int.TryParse(SIM.Utilidades.Data.ObtenerValorParametro("IdFormaReposiciones").ToString(), out idForma);


                Permisos permisos = new Permisos();
                permisosRolModel = permisos.ObtenerPermisosRolForma(idForma, idUsuario);
             
            }

            ViewBag.CodFuncionario = codFuncionario;
            return View(permisosRolModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: ControlVigilancia/GetReporte
        public ActionResult GetReporte()
        {

            PermisosRolModel permisosRolModel = new PermisosRolModel { CanDelete=false, CanInsert=false, CanPrint=false, CanRead= false, CanUpdate = false, IdRol = 0 };

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);

                Permisos permisos = new Permisos();
                permisosRolModel = permisos.ObtenerPermisosRolForma(10564, idUsuario);

            }

            ViewBag.CodFuncionario = codFuncionario;
            return View(permisosRolModel);
        }

        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="CodTramite">Codigo del trámite en el sistema</param>
        /// <param name="CodDocumento">Numero de documento dentro del trámite</param>
        /// <returns></returns>
        [HttpGet, ActionName("LeeDoc")]
        public async Task<ActionResult> GetArchivo(long CodTramite, long CodDocumento)
        {
            if (CodTramite >0  && CodDocumento > 0)
            {
                MemoryStream oStream = await SIM.Utilidades.Archivos.AbrirDocumento(CodTramite, CodDocumento);
                if (oStream != null && oStream.Length > 0)
                {
                    oStream.Position = 0;
                    var Archivo = oStream.GetBuffer();
                    return File(Archivo, "application/pdf");
                }
            }
            return null;
        }
    }
}