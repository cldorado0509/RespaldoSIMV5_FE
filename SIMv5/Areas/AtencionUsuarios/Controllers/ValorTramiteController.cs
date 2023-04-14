using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.AtencionUsuarios.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ValorTramiteController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();


        // GET: AtencionUsuarios/ValorTramite
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene los soportes existentes en el sistema
        /// </summary>
        /// <param name="IdCalculo"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtieneSoporte")]
        public async Task<ActionResult> GetObtieneSoporte(decimal IdCalculo)
        {
            if (IdCalculo <= 0) return null;
            try
            {
                MemoryStream _Ms = new MemoryStream();
                var _Elaboracion = dbSIM.TBTARIFAS_CALCULO.Where(w => w.ID_CALCULO == IdCalculo).Select(s => s.FECHA).FirstOrDefault();
                if (_Elaboracion != null)
                {
                    string _RutaArchSopValorAuto = SIM.Utilidades.Data.ObtenerValorParametro("ArchivosValorAuto").ToString();
                    FileInfo _Soporte = new FileInfo(_RutaArchSopValorAuto + _Elaboracion.ToString("MMyyyy") + @"\" + IdCalculo.ToString() + ".pdf");
                    if (_Soporte.Exists) {
                        using (var stream = new FileStream(_Soporte.FullName, FileMode.Open))
                        {
                            await stream.CopyToAsync(_Ms);
                        }
                        if (_Ms.Length > 0)
                        {
                            _Ms.Position = 0;
                            var Archivo = _Ms.GetBuffer();
                            return File(Archivo, "application/pdf");
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }
            return null;
        }

    }
}