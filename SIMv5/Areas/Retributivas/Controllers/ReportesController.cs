using SIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Retributivas.Controllers
{
    public class ReportesController : Controller
    {

        EntitiesControlOracle db = new EntitiesControlOracle();

        // GET: Retributivas/Reportes
        public ActionResult Index()
        {
            return View();
        }
    }
}