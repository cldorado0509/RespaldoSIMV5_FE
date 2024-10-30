using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using SIM.Areas.REDRIO.Models;


namespace SIM.Areas.REDRIO.Controllers
{
    public class REDRIOController : Controller
    {
        public ActionResult Index()
        {
            return View(); // Renderiza la vista Index
        }

        public ActionResult REDRIO()
        {
            return View(); // Renderiza la vista REDRIO
        }
           public ActionResult Municipio()
        {
            return View();
        }
          public ActionResult Departamento()
        {
            return View();
        }
         public ActionResult Campañas()
        {
            return View();
        }
         public ActionResult ReporteSuperficial()
        {
            return View();
        }
         public ActionResult Estacion()
        {
            return View();
        }

        
    }
}
