using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Configuration;
using SIM.Areas.Seguridad.Controllers;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Newtonsoft.Json;
using SIM.Utilidades;
using SIM.Data;
using SIM.Data.Control;
using SIM.Models;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    
    public class FacturacionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize(Roles = "VFACTURACION")]
        public ActionResult Index()
        {
            return View();
        }
	}
}