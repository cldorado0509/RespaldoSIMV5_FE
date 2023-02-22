using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.Pdf;
using SIM.Data;
using SIM.Areas.Seguridad.Models;
using System.Security.Claims;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using SIM.Areas.Tramites.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace SIM.Areas.AtencionUsuarios.Controllers
{
    public class ValorAutoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public ActionResult Index()
        {
            ViewBag.tiposTramite = JsonConvert.SerializeObject(ModelsToListTramites.GetTiposTramite());

            return View();
        }

        [HttpGet, ActionName("PrintCalculoTramite")]
        public HttpResponseMessage PrintCalculoTramite(int id)
        {
            /*var tramiteE = dbSIM.TBTARIFAS_CALCULO.FirstOrDefault(f => f.ID_CALCULO == id);
            var tramiteS = dbSIM.TBTARIFAS_CALCULO.FirstOrDefault(f => f.ID_CALCULO == (id+1));

            var report = new CalculoReport();
            report.CargarDatos(tramiteE, tramiteS);

            var stream = new MemoryStream();

            report.ExportToPdf(stream);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(stream.ToArray());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;*/
            return null;
        }
    }
}
