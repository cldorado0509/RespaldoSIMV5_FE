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

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class PrestamoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize(Roles = "VPRESTAMO")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "VPRESTAMO")]
        public ActionResult ConsultaPrestamos()
        {
            return View();
        }
    }
}