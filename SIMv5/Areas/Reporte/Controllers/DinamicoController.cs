using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.XtraGrid;

namespace SIM.Areas.Reporte.Controllers
{
    public class DinamicoController : Controller
    {
        //
        // GET: /Reporte/Dinamico/
        [Authorize]
        public ActionResult Index(int? idReporte, int? idParam1, int? idParam2, int? idParam3, int? idParam4, int? idParam5, int? idParam6, int? idParam7, int? idParam8, int? idParam9, int? idParam10)
        {
            ViewBag.IdReporte = idReporte;
            ViewBag.Param1 = idParam1;
            ViewBag.Param2 = idParam2;
            ViewBag.Param3 = idParam3;
            ViewBag.Param4 = idParam4;
            ViewBag.Param5 = idParam5;
            ViewBag.Param6 = idParam6;
            ViewBag.Param7 = idParam7;
            ViewBag.Param8 = idParam8;
            ViewBag.Param9 = idParam9;
            ViewBag.Param10 = idParam10;

            return View();
        }
	}
}