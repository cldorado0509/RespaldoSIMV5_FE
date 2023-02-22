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

namespace SIM.Areas.Documento.Controllers
{

    public class DocumentoController : Controller
    {
        EntitiesControlOracle db = new EntitiesControlOracle();

        
        public ActionResult Ayudas()
        {
            //GEO
            var id_Ayuda = Request.Params["Id_Ayuda"];
            ViewBag.id_Ayuda = id_Ayuda;
            return View();
        }


        public ActionResult ConsultarArbol()
        {
            ObjectParameter rESP = new ObjectParameter("rESP", typeof(string));
            db.SP_AYUDA_TXT1(rESP);
            return Json(rESP.Value);
        }

        public ActionResult Index()
        {
            //GEO
            
            return View();
        }

        public ActionResult CargarDocumentoAdjunto()
        {
            //GEO

            return View();
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
