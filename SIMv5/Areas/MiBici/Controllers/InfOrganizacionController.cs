namespace SIM.Areas.MiBici.Controllers
{
    using DevExpress.Data.ODataLinq.Helpers;
    using SIM.Areas.Seguridad.Models;
    using SIM.Data;
    using SIM.Models;
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Security.Claims;
    using System.Web.Mvc;

    public class InfOrganizacionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle db = new EntitiesControlOracle();
              
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        int idTercero = 0;
    

        // GET: MiBici/InfOrganizacion
        public ActionResult Edit()
        {
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

           
            String sql = "Select ID_INSTALACION,S_NOMBRE from VW_INSTALACION  where ID_TERCERO=" + idTercero;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            ViewBag.json = jSONOUT.Value.ToString();
            ViewBag.IdTercero = idTercero;
         

            return View();
        }


        public ActionResult RegistroVentas ()
        {
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }


            String sql = "Select ID_INSTALACION,S_NOMBRE from VW_INSTALACION  where ID_TERCERO=" + idTercero;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            ViewBag.json = jSONOUT.Value.ToString();
            ViewBag.IdTercero = idTercero;


            return View();
        }


        public ActionResult RegistroEventos()
        {
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }


            String sql = "Select ID_INSTALACION,S_NOMBRE from VW_INSTALACION  where ID_TERCERO=" + idTercero;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            ViewBag.json = jSONOUT.Value.ToString();
            ViewBag.IdTercero = idTercero;


            return View();
        }
    }

        
    
}