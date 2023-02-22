using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Retributivas.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Retributivas/Administrador
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }        
        
        public ActionResult Maestras()
        {
            return View();
        }
    }
}