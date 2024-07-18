using SIM.Data;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.ProcesosJudiciales.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcesosJudicialesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string _rutaPlantillas;
        private string _rutaBase;

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public ProcesosJudicialesController()
        {
            _rutaPlantillas =  SIM.Utilidades.Data.ObtenerValorParametro("UrlObtenerPlantillaLocal").ToString();
            _rutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
            ViewBag.Token = _token;
            ViewBag.RutaPlantillas = _rutaPlantillas;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtraJudiciales()
        {
            var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
            ViewBag.Token = _token;
            ViewBag.RutaPlantillas = _rutaPlantillas;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tra"></param>
        /// <returns></returns>
        /// <exception cref="HttpException"></exception>
        [System.Web.Http.HttpPost]
        public ActionResult CargarArchivoTemp(int Tra)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            if (idUsuario == 0)
            {
                throw new HttpException("El Usuario no está Autenticado");
            }
            string _Ruta = _rutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
            if (!Directory.Exists(_Ruta)) Directory.CreateDirectory(_Ruta);
            var httpRequest = context.Request;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                string filePath = _Ruta + @"\ProcesoJudicial-"  + idUsuario.ToString() + "-" +  Tra.ToString()  + "-" + postedFile.FileName;
                postedFile.SaveAs(filePath);
            }
            return new EmptyResult();
        }
    }
}




