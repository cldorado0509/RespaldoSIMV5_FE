using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Data;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Web.Hosting;
using System.IO;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador para la administración de Terceros
    /// </summary>
    public class AcordeonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsultarJson()
        {
            // Obtener Json que se va a mostrar. En este caso se carga un texto fijo como ejemplo.
            string jsonRespuesta = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/JsonPrueba.txt"));
            return Json(jsonRespuesta);
        }

        public ActionResult GuardarAcordeon()
        {
            string jsonInfo = Request.Params["jsonInfo"];

            // Se procesa y almacena el JSON

            return Content("OK");
        }
    }
}