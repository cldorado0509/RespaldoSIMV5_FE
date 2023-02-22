using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.EnCicla.Models;
using SIM.Data;
using SIM.Areas.General.Models;

namespace SIM.Areas.EnCicla.Controllers
{
    public class ConsultaController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema EnCila
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesEnCiclaSQL dbSIMEnCicla = new EntitiesEnCiclaSQL();

        [Authorize(Roles = "VENCICLA")]
        public ActionResult Prestamo()
        {
            return View("ConsultaPrestamo");
        }

        public ActionResult TestExterno()
        {
            return View();
        }
	}
}