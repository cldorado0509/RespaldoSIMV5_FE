using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Capacitacion.Controllers
{
    [Authorize]
    public class CapacitacionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();


        // GET: Capacitacion/Capacitacion
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, ActionName("ReporteInscritos")]
        public ActionResult GetReporteInscritos(long id)
        {
            if (id == 0) return null;
            MemoryStream oStream = new MemoryStream();
            string _Reporte = Server.MapPath("~/Areas/Capacitacion/Reportes/Paticipacapacitacion.repx");
            var Inscritos = (from Par in dbSIM.PARTICIPANTE
                             join Ins in dbSIM.EVENTOPARTICIPANTES on Par.ID_PARTICIPANTE equals Ins.ID_PARTICIPANTE
                             join Eve in dbSIM.EVENTO on Ins.ID_EVENTO equals Eve.ID_EVENTO
                             where Eve.ID_EVENTO == id
                             select new
                             {
                                 Par.ID_PARTICIPANTE,
                                 Par.N_PARTICIPANTE,
                                 Par.S_NOMBRE,
                                 Par.S_APELLIDO,
                                 Par.S_EMPRESA,
                                 Par.S_TELEFONO,
                                 NOMBREEVENTO = Eve.S_EVENTO,
                                 LUGAR = Eve.S_LUGAR,
                                 FECHA = Eve.D_EVENTO,
                                 Eve.N_DURACION,
                                 Eve.S_RESPONSABLE,
                                 Eve.N_CAPACIDAD,
                                 Eve.S_CONTACTO,
                                 Eve.S_CORREOCONTACTO,
                                 Par.S_CORREOELECTRONICO,
                                 Par.S_SECTOR,
                                 Par.ID_TERCERO
                             }).ToList();
            if (Inscritos != null)
            {
                string jsonResults = JsonConvert.SerializeObject(Inscritos);
                DataTable TablaInscritos = JsonConvert.DeserializeObject<DataTable>(jsonResults);
                XtraReport Rpt = new XtraReport();
                Rpt.LoadLayout(_Reporte);
                Rpt.DataSource = TablaInscritos;
                Rpt.ExportToPdf(oStream);
                oStream.Seek(0, SeekOrigin.Begin);
            }
            if (oStream != null && oStream.Length > 0)
            {
                var Descarga = new FileStreamResult(oStream, "application/pdf");
                return Descarga;
            }
            return null;
        }
    }
}