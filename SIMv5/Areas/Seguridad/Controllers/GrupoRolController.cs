using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador GrupoRol: Creación, modificación, borrado y consulta de los registros asociados a la relacion de los roles de un grupo
    /// </summary>
    public class GrupoRolController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad GrupoRol
        /// </summary>
        /// <returns>Vista de administración de GrupoRol</returns>
        [Authorize(Roles = "VGRUPOROL")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de GrupoRol
        /// </summary>
        /// <returns>Vista de Consulta de GrupoRol</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VGRUPOROL")]
        public ActionResult gvwAdministrarGrupoRol()
        {
            var model = dbSeguridad.GRUPO_ROL;
            return PartialView("_gvwAdministrarGrupoRol", model.ToList());
        }

        /// <summary>
        /// Crea un GrupoRol 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del GrupoRol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CGRUPOROL")]
        public ActionResult gvwAdministrarGrupoRolCrear(GRUPO_ROL item)
        {
            var model = dbSeguridad.GRUPO_ROL;
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
            return PartialView("_gvwAdministrarGrupoRol", model.ToList());
        }

        /// <summary>
        /// Actualiza un GrupoRol con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del GrupoRol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AGRUPOROL")]
        public ActionResult gvwAdministrarGrupoRolActualizar(GRUPO_ROL item)
        {
            var model = dbSeguridad.GRUPO_ROL;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_GRUPO_ROL == item.ID_GRUPO_ROL);
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
            return PartialView("_gvwAdministrarGrupoRol", model.ToList());
        }

        /// <summary>
        /// Elimina un GrupoRol
        /// </summary>
        /// <param name="ID_CARGO">Identificador del GrupoRol a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EGRUPOROL")]
        public ActionResult gvwAdministrarGrupoRolEliminar(System.Int32 ID_GRUPO_ROL)
        {
            var model = dbSeguridad.GRUPO_ROL;
            if (ID_GRUPO_ROL != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_GRUPO_ROL == ID_GRUPO_ROL);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarGrupoRol", model.ToList());
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
