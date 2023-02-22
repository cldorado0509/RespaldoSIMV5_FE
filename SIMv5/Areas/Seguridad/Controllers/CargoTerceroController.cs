using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.Seguridad.Models;
using DevExpress.Web.Mvc;
using SIM.Data;
using SIM.Data.Seguridad;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador CargoTercero: Creación, modificación, borrado y consulta de los registros asociados a la relacion de los cargos de un tercero
    /// </summary>
    public class CargoTerceroController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbseguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad CargoTercero
        /// </summary>
        /// <returns>Vista de administración de CargoTercero</returns>
        [Authorize(Roles = "VCARGOTERCERO")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de CargoTercero
        /// </summary>
        /// <returns>Vista de Consulta de CargoTercero</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VCARGOTERCERO")]
        public ActionResult gvwAdministrarCargoTercero()
        {
            var model = dbseguridad.CARGO_TERCERO;
            return PartialView("_gvwAdministrarCargoTercero", model.ToList());
        }

        /// <summary>
        /// Crea un CargoTercero 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del CargoTercero</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CCARGOTERCERO")]
        public ActionResult gvwAdministrarCargoTerceroCrear(CARGO_TERCERO item)
        {
            var model = dbseguridad.CARGO_TERCERO;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    dbseguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return PartialView("_gvwAdministrarCargoTercero", model.ToList());
        }

        /// <summary>
        /// Actualiza un CargoTercero con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del CargoTercero</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ACARGOTERCERO")]
        public ActionResult gvwAdministrarCargoTerceroActualizar(CARGO_TERCERO item)
        {
            var model = dbseguridad.CARGO_TERCERO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_CARGO_TERCERO == item.ID_CARGO_TERCERO);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        dbseguridad.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return PartialView("_gvwAdministrarCargoTercero", model.ToList());
        }

        /// <summary>
        /// Elimina un CargoTercero
        /// </summary>
        /// <param name="ID_CARGO">Identificador del CargoTercero a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ECARGOTERCERO")]
        public ActionResult gvwAdministrarCargoTerceroEliminar(System.Int32 ID_CARGO_TERCERO)
        {
            var model = dbseguridad.CARGO_TERCERO;
            if (ID_CARGO_TERCERO != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_CARGO_TERCERO == ID_CARGO_TERCERO);
                    if (item != null)
                        model.Remove(item);
                    dbseguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarCargoTercero", model.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbseguridad.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
