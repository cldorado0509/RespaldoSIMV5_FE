using SIM.Areas.Seguridad.Models;
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
            _rutaPlantillas =  SIM.Utilidades.Data.ObtenerValorParametro("UrlObtenerPlantilla").ToString();
            _rutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var _IdUsuario = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("nameidentifier")).FirstOrDefault();
            decimal IdUsuario = 0;
            decimal.TryParse(_IdUsuario.Value, out IdUsuario);

            var fun = (from uf in dbSIM.USUARIO_FUNCIONARIO
                       join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                       where uf.ID_USUARIO == IdUsuario
                       select f.NOMBRES + " " + f.APELLIDOS).FirstOrDefault();


            string actionName = RouteData.Values["action"].ToString();
            string controllerName = RouteData.Values["controller"].ToString();
            string areaName = RouteData.DataTokens["area"].ToString();
            PermisosRolModel permisosRolModel = SIM.Utilidades.Security.PermisosFormulario(areaName, controllerName, actionName, IdUsuario);



            var _claim = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
            ViewBag.Token = _claim.Value;
            ViewBag.RutaPlantillas = _rutaPlantillas;
            ViewBag.Funcionario = fun;
            return View(permisosRolModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtraJudiciales()
        {
            var _IdUsuario = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("nameidentifier")).FirstOrDefault();
            decimal IdUsuario = 0;
            decimal.TryParse(_IdUsuario.Value, out IdUsuario);

            var fun = (from uf in dbSIM.USUARIO_FUNCIONARIO
                       join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                       where uf.ID_USUARIO == IdUsuario
                       select f.NOMBRES + " " + f.APELLIDOS).FirstOrDefault();


            string actionName = RouteData.Values["action"].ToString();
            string controllerName = RouteData.Values["controller"].ToString();
            string areaName = RouteData.DataTokens["area"].ToString();

            PermisosRolModel permisosRolModel = SIM.Utilidades.Security.PermisosFormulario(areaName, controllerName, actionName, IdUsuario);


            var _claim = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
            ViewBag.Token = _claim.Value;
            ViewBag.Funcionario = fun;
            ViewBag.RutaPlantillas = _rutaPlantillas;
            return View(permisosRolModel);
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




