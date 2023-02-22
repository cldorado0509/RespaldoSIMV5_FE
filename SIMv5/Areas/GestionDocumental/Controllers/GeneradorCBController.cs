using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.BarCodes;
using System.IO;
using SIM.Areas.GestionDocumental.Models;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class GeneradorCBController : Controller
    {
        //
        // GET: /GeneradorCB/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerarCB(string codigoBarras, string texto)
        {
            var report = new CBReport();
            report.CB = codigoBarras;
            report.Texto1 = texto;

            var stream = new MemoryStream();
            report.ExportToPdf(stream);
            report.Dispose();

            return File(stream.GetBuffer(), "application/pdf");
        }
	}
}