using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.GestionDocumental.Models;
using DevExpress.Web.Mvc;
using System.Xml.Linq;
using SIM.Data;
using Newtonsoft.Json;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class RadicadorController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        // tipoRadicador: 1: Unidades Documentales Simples, 2: Unidades Documentales Complejas
        [Authorize(Roles = "VRADICADOR")]
        public ActionResult Index(int? tipoRadicador)
        {
            //ViewBag.TipoRadicador = tipoRadicador == null || tipoRadicador < 1 || tipoRadicador > 2 ? 2 : tipoRadicador;
            ViewBag.TipoRadicador = 2;
            ViewBag.tiposAnexo = JsonConvert.SerializeObject(ModelsToListGestionDocumental.GetTiposAnexo());

            return View();
        }

        [Authorize(Roles = "VRADICADOR")]
        public ActionResult Foliar()
        {
            return View();
        }
    }
}