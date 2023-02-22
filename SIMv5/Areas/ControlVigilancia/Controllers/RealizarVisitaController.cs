using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SIM.Data;
using SIM.Models;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    public class RealizarVisitaController : Controller
    {
        EntitiesControlOracle db = new EntitiesControlOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
 
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConsultarInformacionIndustria()
        {

            Decimal idInstalacion = Decimal.Parse(Request.Params["ins"]);
            Decimal idTercero = Decimal.Parse(Request.Params["tercero"]);
            Decimal idVisita = Decimal.Parse(Request.Params["visita"]);

            ObjectParameter jsonOut = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_INFO_INDUSTRIA(idInstalacion, idTercero, idVisita, jsonOut);
            return Json(jsonOut.Value);
        }

        public ActionResult setInformacionIndustria()
        {

            Decimal idInstalacion = Decimal.Parse(Request.Params["ins"]);
            Decimal idTercero = Decimal.Parse(Request.Params["tercero"]);
            Decimal tipo = Decimal.Parse(Request.Params["tipo"]);
            Decimal idVisita = Decimal.Parse(Request.Params["visita"]);
            string jsonInfo = Request.Params["jsonInf"];
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));

            db.SP_SET_INFO_INDUSTRIA(idInstalacion, idTercero, tipo, idVisita, jsonInfo, rta);
            return Json(rta.Value);


        }
        public ActionResult consultarSector()
        {
            String sql = "select id_sector,sector from general.QRY_SECTOR";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);


        }
	}
}