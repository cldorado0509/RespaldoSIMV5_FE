using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class DocumentosController : Controller
    {
        // GET: GestionDocumental/Documentos
        public ActionResult BuscarDocumentos()
        {
            return View();
        }
    }
}