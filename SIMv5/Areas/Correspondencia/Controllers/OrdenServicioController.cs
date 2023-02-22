using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Correspondencia.Controllers
{
    public class OrdenServicioController : Controller
    {
        [Authorize(Roles = "VORDENSERVICIO")]
        // GET: Correspondencia/OrdenServicio
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "VORDENSERVICIO")]
        // GET: Correspondencia/OrdenServicio/Consulta
        public ActionResult Consulta()
        {
            return View();
        }
        [Authorize(Roles = "VORDENSERVICIO")]
        // GET: Correspondencia/OrdenServicio/Devoluciones
        public ActionResult Devoluciones()
        {
            return View();
        }
    }
}