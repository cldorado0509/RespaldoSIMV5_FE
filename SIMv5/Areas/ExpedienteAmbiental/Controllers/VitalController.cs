namespace SIM.Areas.ExpedienteAmbiental.Controllers
{
    using SIM.Data;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class VitalController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        ///  GET: ExpedienteAmbiental/Vital
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
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

            return View();
        }
    }
}