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
    /// Controlador CargoRol: Creación, modificación, borrado y consulta de los registros asociados a la relacion de los roles de un cargo
    /// </summary>
    public class CargoRolController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad CargoRol
        /// </summary>
        /// <returns>Vista de administración de CargoRol</returns>
        [Authorize(Roles = "VCARGOROL")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de CargoRol
        /// </summary>
        /// <returns>Vista de Consulta de CargoRol</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VCARGOROL")]
        public ActionResult gvwAdministrarCargoRol()
        {
            var model = dbSeguridad.CARGO_ROL;
            return PartialView("_gvwAdministrarCargoRol", model.ToList());
        }

        /// <summary>
        /// Crea un CargoRol 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del CargoRol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CCARGOROL")]
        public ActionResult gvwAdministrarCargoRolCrear(CARGO_ROL item)
        {
            var model = dbSeguridad.CARGO_ROL;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return PartialView("_gvwAdministrarCargoRol", model.ToList());
        }

        /// <summary>
        /// Actualiza un CargoRol con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del CargoRol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ACARGOROL")]
        public ActionResult gvwAdministrarCargoRolActualizar(CARGO_ROL item)
        {
            var model = dbSeguridad.CARGO_ROL;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_CARGO_ROL == item.ID_CARGO_ROL);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        dbSeguridad.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return PartialView("_gvwAdministrarCargoRol", model.ToList());
        }

        /// <summary>
        /// Elimina un CargoRol
        /// </summary>
        /// <param name="ID_CARGO">Identificador del CargoRol a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ECARGOROL")]
        public ActionResult gvwAdministrarCargoRolEliminar(System.Int32 ID_CARGO_ROL)
        {
            var model = dbSeguridad.CARGO_ROL;
            if (ID_CARGO_ROL != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_CARGO_ROL == ID_CARGO_ROL);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarCargoRol", model.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbSeguridad.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
