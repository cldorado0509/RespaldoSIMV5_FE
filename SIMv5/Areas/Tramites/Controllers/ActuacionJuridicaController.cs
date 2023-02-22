using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.Tramites.Models;
using SIM.Areas.Models;
using SIM.Data;

namespace SIM.Areas.Tramites.Controllers
{
    public class ActuacionJuridicaController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public ActionResult Index()
        {
            return View();
        }
    }
}
