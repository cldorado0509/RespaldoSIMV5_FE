using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Data;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using System.Security.Claims;
using SIM.Data.General;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador Tercero: Creación, modificación, borrado y consulta de Terceros
    /// </summary>
    public class BusquedaController : Controller
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Método por defecto del controlador. Carga la vista de Consulta de Terceros
        /// </summary>
        /// <returns>Vista de Consulta de Terceros</returns>
        //[Authorize(Roles = "VTERCERO")]
        public ActionResult Index()
        {
            dynamic modelData;
            var model = from busqueda in dbSIM.BUSQUEDA
                        select busqueda;

            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamicaFullText(modelData, "S_TEXTO,rene");

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(0).Take(10).ToList();

            //return resultado;
            return View();
        }

        /// <summary>
        /// Carga modelo del tercero seleccionado
        /// </summary>
        /// <param name="id">ID del Tercero Seleccionado. Si es NULL, significa que se va a crear un Tercero</param>
        /// <param name="tipoTercero">Tipo de Tercero para creación: "N" Natural, "J" Jurídico</param>
        /// <returns>Vista de Consulta de detalle de Tercero</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VTERCERO")]
        public ActionResult Tercero(int? id, string tipoTercero, bool? vistaRetorno)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            ViewBag.idTercero = (id == null ? 0 : id);

            if (tipoTercero == null)
            {
                if (id != null) // Si id es null el tercero es nuevo de lo contrario debería existir
                {
                    TERCERO tercero = dbSIM.TERCERO.Find(id);
                    tipoTercero = (tercero.ID_TIPODOCUMENTO == 2 ? "J" : "N");
                }
                else
                {
                    var administrador = false;

                    if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
                    {
                        administrador = claimPpal.IsInRole("XTERCERO");
                    }

                    if (!administrador)
                    {
                        // Si el usuario no es administrador y ya tiene un tercero asignado, edita el tercero en vez de crear uno nuevo
                        if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                        {
                            id = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                            TERCERO tercero = dbSIM.TERCERO.Find(id);
                            tipoTercero = (tercero.ID_TIPODOCUMENTO == 2 ? "J" : "N");
                            ViewBag.idTercero = id;
                        }
                        else
                        {
                            var userId = Convert.ToInt32(User.Identity.GetUserId());

                            // Si el tercero es nuevo, verificamos en el usuario el tipo de persona y de acuerdo a esto establecemos el tipo de tercero
                            var tipoUsuario = (from usuario in dbSIM.USUARIO
                                               where usuario.ID_USUARIO == userId
                                               select new
                                               {
                                                   usuario.S_TIPO,
                                                   usuario.S_NOMBRES,
                                                   usuario.S_APELLIDOS,
                                                   usuario.S_EMAIL
                                               }
                                              ).FirstOrDefault();

                            if (tipoUsuario == null)
                                tipoTercero = "N";
                            else
                            {
                                tipoTercero = tipoUsuario.S_TIPO;

                                ViewBag.nombres = tipoUsuario.S_NOMBRES;
                                ViewBag.apellidos = tipoUsuario.S_APELLIDOS;
                                ViewBag.email = tipoUsuario.S_EMAIL;
                            }
                        }
                    }
                }
            }

            ViewBag.tipoTercero = tipoTercero;
            ViewBag.vistaRetorno = (vistaRetorno == null ? "N" : ((bool)vistaRetorno ? "S" : "N"));

            if (tipoTercero == "N")
            {
                ViewBag.tiposIdentificacion = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoNatural());
                ViewBag.tiposIdentificacionNatural = ViewBag.tiposIdentificacion;
            }
            else
            {
                ViewBag.tiposIdentificacion = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoJuridica());
                ViewBag.tiposIdentificacionNatural = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoNatural());
            }

            return View();
        }

        public ActionResult TerceroInformacionGeneral(int? id, string tipoTercero)
        {
            ViewBag.idTercero = id;
            ViewBag.tipoTercero = tipoTercero;

            return PartialView();
        }

        public ActionResult TerceroContactos(int? id, string tipoTercero)
        {
            ViewBag.idTercero = id;
            ViewBag.tipoTercero = tipoTercero;

            return PartialView();
        }

        public ActionResult TerceroInstalaciones(int? id, string tipoTercero)
        {
            ViewBag.idTercero = id;
            ViewBag.tipoTercero = tipoTercero;

            return PartialView();
        }

        public ActionResult TerceroUsuarios(int? id, string tipoTercero)
        {
            ViewBag.idTercero = id;
            ViewBag.tipoTercero = tipoTercero;

            return PartialView();
        }
    }
}