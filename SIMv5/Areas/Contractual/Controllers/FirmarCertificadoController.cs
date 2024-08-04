using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.Contractual.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FirmarCertificadoController : Controller
    {
        string urlApiContractual = SIM.Utilidades.Data.ObtenerValorParametro("UrlMicroSitioContractual").ToString();

        // GET: Contractual/FirmarCertificado
        public ActionResult Index()
        {
            var _claim = (User.Identity as ClaimsIdentity).Claims.Where(w => w.Type.EndsWith("nameidentifier")).FirstOrDefault();
            ViewBag.idusuario = _claim.Value;
            return View();
        }
    }
}