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
    /// Controlador RolForma: Creación, modificación, borrado y consulta de los registros asociados a la relacion de un rol con los items del menu
    /// </summary>
    public class RolFormaController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad RolForma
        /// </summary>
        /// <returns>Vista de administración de RolForma</returns>
        [Authorize(Roles = "VROLFORMA")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de RolForma
        /// </summary>
        /// <returns>Vista de Consulta de RolForma</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VROLFORMA")]
        public ActionResult gvwAdministrarRolForma()
        {
            var model = dbSeguridad.ROL_FORMA;
            return PartialView("_gvwAdministrarRolForma", model.ToList());
        }

        /// <summary>
        /// Consulta y carga Vista con el TreeView de RolForma
        /// </summary>
        /// <returns>Vista de Consulta de RolForma</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VROLFORMA")]
        public ActionResult tvwListarRolForma()
        {
            return PartialView("_tvwListarRolForma", ModelsToListSeguridad.GetRolForma());
        }

        /// <summary>
        /// Crea un RolForma 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del RolForma</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CROLFORMA")]
        public ActionResult gvwAdministrarRolFormaCrear(ROL_FORMA item)
        {
            var model = dbSeguridad.ROL_FORMA;
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
            return PartialView("_gvwAdministrarRolForma", model.ToList());
        }

        /// <summary>
        /// Actualiza un RolForma con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del RolForma</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AROLFORMA")]
        public ActionResult gvwAdministrarRolFormaActualizar(ROL_FORMA item)
        {
            var model = dbSeguridad.ROL_FORMA;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_ROL_FORMA == item.ID_ROL_FORMA);
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
            return PartialView("_gvwAdministrarRolForma", model.ToList());
        }

        /// <summary>
        /// Elimina un RolForma
        /// </summary>
        /// <param name="ID_CARGO">Identificador del RolForma a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EROLFORMA")]
        public ActionResult gvwAdministrarRolFormaEliminar(System.Int32 ID_ROL_FORMA)
        {
            var model = dbSeguridad.ROL_FORMA;
            if (ID_ROL_FORMA != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_ROL_FORMA == ID_ROL_FORMA);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarRolForma", model.ToList());
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
