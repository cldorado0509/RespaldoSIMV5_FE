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
    /// Controlador PermisoObjeto: Creación, modificación, borrado y consulta de los permisos que se un usuario pueda terner sobre un objeto
    /// </summary>
    public class PermisoObjetoController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad PermisoObjeto
        /// </summary>
        /// <returns>Vista de administración de PermisoObjeto</returns>
        [Authorize(Roles = "VPERMISOOBJETO")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de PermisoObjeto
        /// </summary>
        /// <returns>Vista de Consulta de PermisoObjeto</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VPERMISOOBJETO")]
        public ActionResult gvwAdministrarPermisoObjeto()
        {
            var model = dbSeguridad.PERMISO_OBJETO;
            return PartialView("_gvwAdministrarPermisoObjeto", model.ToList());
        }

        /// <summary>
        /// Crea un PermisoObjeto 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del PermisoObjeto</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CPERMISOOBJETO")]
        public ActionResult gvwAdministrarPermisoObjetoCrear(PERMISO_OBJETO item)
        {
            var model = dbSeguridad.PERMISO_OBJETO;
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
            return PartialView("_gvwAdministrarPermisoObjeto", model.ToList());
        }

        /// <summary>
        /// Actualiza un PermisoObjeto con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del PermisoObjeto</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "APERMISOOBJETO")]
        public ActionResult gvwAdministrarPermisoObjetoActualizar(PERMISO_OBJETO item)
        {
            var model = dbSeguridad.PERMISO_OBJETO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_PERMISOOBJETO == item.ID_PERMISOOBJETO);
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
            return PartialView("_gvwAdministrarPermisoObjeto", model.ToList());
        }

        /// <summary>
        /// Elimina un PermisoObjeto
        /// </summary>
        /// <param name="ID_CARGO">Identificador del PermisoObjeto a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EPERMISOOBJETO")]
        public ActionResult gvwAdministrarPermisoObjetoEliminar(System.Int32 ID_PERMISOOBJETO)
        {
            var model = dbSeguridad.PERMISO_OBJETO;
            if (ID_PERMISOOBJETO != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_PERMISOOBJETO == ID_PERMISOOBJETO);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarPermisoObjeto", model.ToList());
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
