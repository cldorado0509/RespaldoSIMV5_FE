using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Data;
using System.Data;
using SIM.Areas.Seguridad.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using SIM.Areas.General.Models;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador Instalacion: Creación, modificación, borrado y consulta de Instalaciones
    /// </summary>
    public class InstalacionController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Método por defecto del controlador. Carga la vista de Consulta de Instalaciones
        /// </summary>
        /// <returns>Vista de Consulta de Instalaciones</returns>
        //[Authorize(Roles = "VINSTALACION")]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Instalacion(int? id, int? idTercero, int? T, int? I)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XINSTALACION");
            }

            if (administrador)
            {
                if (idTercero != null)
                    ViewBag.idTercero = idTercero;
                else
                {
                    if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                        ViewBag.idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                    else
                        ViewBag.idTercero = 0;
                }
            }
            else
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                    ViewBag.idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                else
                    ViewBag.idTercero = 0;
            }

            ViewBag.T = T;
            ViewBag.I = I;
            ViewBag.idInstalacion = id;
            ViewBag.municipios = JsonConvert.SerializeObject(ModelsToListGeneral.GetMunicipios());
            ViewBag.tiposVia = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposVia());
            ViewBag.letrasVia = JsonConvert.SerializeObject(ModelsToListGeneral.GetLetrasVia());
            ViewBag.tiposInstalaciones = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposInstalaciones());
            //ViewBag.tiposDeclaraciones = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDeclaraciones());
            //ViewBag.estados = JsonConvert.SerializeObject(ModelsToListGeneral.GetEstados());

            return View();
        }
    }
}
