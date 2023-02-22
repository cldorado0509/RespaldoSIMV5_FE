using SIM.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Correspondencia.Controllers
{
    public class CorrespondenciaController : Controller
    {
        // GET: Correspondencia/Correspondencia
        [Authorize(Roles ="VCORRESPONDENCIA")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="CodTramite">Codigo del tramite en el sistema</param>
        /// <param name="Coddocuemnto">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        [HttpGet, ActionName("LeeDoc")]
        public async Task<ActionResult> GetArchivo(long CodTramite, long CodDocumento)
        {
            MemoryStream oStream = await SIM.Utilidades.Archivos.AbrirDocumento(CodTramite, CodDocumento);
            if (oStream.Length > 0)
            {
                oStream.Position = 0;
                var Archivo = oStream.GetBuffer();
                return File(Archivo, "application/pdf");
            }
            return null;
        }
        /// <summary>
        /// Consulta un soporte de la orden de servicio y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="IdCodOrden"></param>
        /// <returns></returns>
        [HttpGet, ActionName("LeeSoporte")]
        public ActionResult GetSoporte(long IdCodOrden)
        {
            MemoryStream oStream = SIM.Utilidades.Archivos.AbrirSoporte(IdCodOrden);
            if (oStream != null && oStream.Length > 0)
            {
                oStream.Position = 0;
                var Archivo = oStream.GetBuffer();
                return File(Archivo, "application/pdf");
            }
            return Content("<center><h1>No se encontró un soporte para la Orden de Servicio</h1></center>");
        }
    }
}