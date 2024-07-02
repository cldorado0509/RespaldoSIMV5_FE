using SIM.Data;
using System;
using System.IO;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.ProcesosJudiciales.Controllers
{
    public class ProcesosJudicialesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //[Authorize(Roles = "VPROCESOSJUDICIALES")]
        public ActionResult Index()
        {
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
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
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
            string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
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
