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


namespace SIM.Areas.Flora.Controllers
{
    public class FloraWebAPIController : ApiController
    {

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        //SIM.Areas.Flora.Models.EntitiesFlora dbFlora = new SIM.Areas.Flora.Models.EntitiesFlora();
        //System.Web.HttpContext context = System.Web.HttpContext.Current;


        //RESIDUOS

        //public IHttpActionResult GetGuardarFotografiasFlora(String idFotos)
        //{
        //    string[] arrFotos = idFotos.Split(',');

        //    FOTOGRAFIA_FLORA foto = new FOTOGRAFIA_FLORA();
        //    for (int i = 0; i < arrFotos.Length; i++)
        //    {
        //        foto.ID_FOTOGRAFIA = Convert.ToInt32(arrFotos[i]);
        //        foto.ID_FLORA = Convert.ToInt32(1);
        //        db.FOTOGRAFIA_FLORA.Add(foto);
        //        db.SaveChanges();
        //    }
        //    return Ok();
        //}
    
    }
}