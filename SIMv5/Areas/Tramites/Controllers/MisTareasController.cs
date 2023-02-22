namespace SIM.Areas.Tramites.Controllers
{
    using SIM.Data;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    public class MisTareasController : Controller
    {

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        // GET: Tramites/MisTareas
        [Authorize]
        public ActionResult Index()
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

            var DatosTareas = new Tramites.Models.DatosTareasFuncionario();
            DatosTareas.NroTareasPendientes = (from tar in dbSIM.TBTRAMITETAREA
                                               where (tar.COPIA == 0)
                                               && (tar.ESTADO == 0) && (tar.CODFUNCIONARIO == codFuncionario)
                                               select tar).ToList().Count;
            DatosTareas.NroTareasTerminadas = (from tar in dbSIM.TBTRAMITETAREA
                                               where (tar.COPIA == 0)
                                               && (tar.ESTADO == 1) && (tar.CODFUNCIONARIO == codFuncionario)
                                               select tar).ToList().Count;
            DatosTareas.NroTareasNoAbiertas = (from tar in dbSIM.TBTRAMITETAREA
                                               where (tar.RECIBIDA != 1) && (tar.COPIA == 0)
                                               && (tar.ESTADO == 0) && (tar.CODFUNCIONARIO == codFuncionario)
                                               select tar).ToList().Count;
            DatosTareas.NroTareasCopiaPendientes = (from tar in dbSIM.TBTRAMITETAREA
                                               where (tar.COPIA == 1) && (tar.ESTADO == 0) && 
                                               (tar.CODFUNCIONARIO == codFuncionario)
                                               select tar).ToList().Count;
            DatosTareas.NroTareasCopiaTerminadas = (from tar in dbSIM.TBTRAMITETAREA
                                                    where (tar.COPIA == 1) && (tar.ESTADO == 1) && 
                                                    (tar.CODFUNCIONARIO == codFuncionario)
                                                    select tar).ToList().Count;
            DatosTareas.NroTareasCopiaNoAbiertas = (from tar in dbSIM.TBTRAMITETAREA
                                                    where (tar.RECIBIDA != 1) && (tar.COPIA == 1)
                                                    && (tar.ESTADO == 0) && (tar.CODFUNCIONARIO == codFuncionario)
                                                    select tar).ToList().Count;

            DatosTareas.CodFuncionario = codFuncionario;  // ModelsToListSeguridad.ObtenerIdFuncionario(User.Identity.GetUserId());
            return View(DatosTareas);
        }
    }
}