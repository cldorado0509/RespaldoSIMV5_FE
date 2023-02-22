using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System.Web;
using DevExpress.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Data.Linq.SqlClient;


namespace SIM.Areas.Ayudas.Controllers
{
    public class AyudasWebAPIController : ApiController
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();


        public IHttpActionResult GetContenido(decimal id)
        {
            var Ayudas = from f in db.VW_AYUDAS
                              where f.ID_AYUDA == id
                              select new { f.CONTENIDO};

            if (Ayudas == null)
            {
                return NotFound();
            }
            return Ok(Ayudas);
        }
    
    }
}