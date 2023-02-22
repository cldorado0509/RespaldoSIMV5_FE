using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using SIM.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using SIM.Models;

namespace SIM.Areas.Ayudas.Controllers
{

    public class AyudasController : Controller
    {
        EntitiesControlOracle db = new EntitiesControlOracle();

        
        public ActionResult Ayudas()
        {
            //GEO
            var id_Ayuda = Request.Params["Id_Ayuda"];
            if (id_Ayuda == null)
                id_Ayuda ="82";
                ViewBag.id_Ayuda = id_Ayuda;
            
            return View();
        }


        public ActionResult ConsultarArbol()
        {
            ObjectParameter jsonOut = new ObjectParameter("jsonOut", typeof(string));
            db.SP_GET_AYUDA(0,jsonOut);
            return Json(jsonOut.Value);
        }


        //public ActionResult GetContenido(decimal id)
        //{
            
        //    var product = from p in db.AYUDAS
        //                  where p.ID_AYUDA == id
        //                  select new { p.CONTENIDO};
        //    return View(product);
        //}
    }
}
