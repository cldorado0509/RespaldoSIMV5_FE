using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.GestionDocumental.Models;
using SIM.Areas.General.Models;
using System.Data;
using System.Globalization;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System.IO;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class DigitalizacionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize(Roles = "VDIGITALIZACION")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "VDIGITALIZACION")]
        public ActionResult CargarDocumento()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                }
            }

            return null;
        }
    }
}