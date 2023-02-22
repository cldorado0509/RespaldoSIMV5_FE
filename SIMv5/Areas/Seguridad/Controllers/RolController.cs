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
    /// Controlador Rol: Creación, modificación, borrado y consulta de los roles del sistema
    /// </summary>
    public class RolController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad Rol
        /// </summary>
        /// <returns>Vista de administración de Rol</returns>
        [Authorize(Roles = "VROL")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Rol
        /// </summary>
        /// <returns>Vista de Consulta de Rol</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VROL")]
        public ActionResult gvwAdministrarRol()
        {
            var model = dbSeguridad.ROL;
            return PartialView("_gvwAdministrarRol", model.ToList());
        }

        /// <summary>
        /// Crea un Rol 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Rol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CROL")]
        public ActionResult gvwAdministrarRolCrear([ModelBinder(typeof(DevExpressEditorsBinder))] ROL item)
        {
            var model = dbSeguridad.ROL;
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
            return PartialView("_gvwAdministrarRol", model.ToList());
        }

        /// <summary>
        /// Actualiza un Rol con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Rol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AROL")]
        public ActionResult gvwAdministrarRolActualizar([ModelBinder(typeof(DevExpressEditorsBinder))] ROL item)
        {
            var model = dbSeguridad.ROL;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_ROL == item.ID_ROL);
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
            return PartialView("_gvwAdministrarRol", model.ToList());
        }

        /// <summary>
        /// Elimina un Rol
        /// </summary>
        /// <param name="ID_CARGO">Identificador del Rol a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EROL")]
        public ActionResult gvwAdministrarRolEliminar(System.Int32 ID_ROL)
        {
            var model = dbSeguridad.ROL;
            if (ID_ROL != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_ROL == ID_ROL);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarRol", model.ToList());
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView del Rol para listar sus elementos
        /// </summary>
        /// <returns>Vista de Consulta del Rol para listar sus elementos</returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult gvwSeleccionarRol()
        {
            var model = from rol in dbSeguridad.ROL
                        select new
                        {
                            rol.ID_ROL,
                            rol.S_NOMBRE
                        };

            return PartialView("_gvwSeleccionarRol", model.ToList());
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
