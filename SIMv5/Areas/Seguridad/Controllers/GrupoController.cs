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
    /// Controlador Grupo: Creación, modificación, borrado y consulta de los grupos
    /// </summary>
    public class GrupoController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad Grupo
        /// </summary>
        /// <returns>Vista de administración de Grupo</returns>
        [Authorize(Roles = "VGRUPO")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Grupo
        /// </summary>
        /// <returns>Vista de Consulta de Grupo</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VGRUPO")]
        public ActionResult gvwAdministrarGrupo()
        {
            var model = dbSeguridad.GRUPO;
            return PartialView("_gvwAdministrarGrupo", model.ToList());
        }

        /// <summary>
        /// Crea un Grupo 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Grupo</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CGRUPO")]
        public ActionResult gvwAdministrarGrupoCrear(GRUPO item)
        {
            var model = dbSeguridad.GRUPO;
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
            return PartialView("_gvwAdministrarGrupo", model.ToList());
        }

        /// <summary>
        /// Actualiza un Grupo con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Grupo</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AGRUPO")]
        public ActionResult gvwAdministrarGrupoActualizar(GRUPO item)
        {
            var model = dbSeguridad.GRUPO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_GRUPO == item.ID_GRUPO);
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
            return PartialView("_gvwAdministrarGrupo", model.ToList());
        }

        /// <summary>
        /// Elimina un Grupo
        /// </summary>
        /// <param name="ID_CARGO">Identificador del Grupo a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EGRUPO")]
        public ActionResult gvwAdministrarGrupoEliminar(System.Int32 ID_GRUPO)
        {
            var model = dbSeguridad.GRUPO;
            if (ID_GRUPO != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_GRUPO == ID_GRUPO);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarGrupo", model.ToList());
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
