using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador Dependencia: Creación, modificación, borrado y consulta de las dependencias
    /// </summary>
    public class DependenciaController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad Dependencia
        /// </summary>
        /// <returns>Vista de administración de Dependencia</returns>
        [Authorize(Roles = "VDEPENDENCIA")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Dependencia
        /// </summary>
        /// <returns>Vista de Consulta de Dependencia</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VDEPENDENCIA")]
        public ActionResult gvwAdministrarDependencia()
        {
            var model = dbSeguridad.DEPENDENCIA;
            return PartialView("_gvwAdministrarDependencia", model.ToList());
        }

        /// <summary>
        /// Crea una Dependencia 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos de la Dependencia</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CDEPENDENCIA")]
        public ActionResult gvwAdministrarDependenciaCrear(DEPENDENCIA item)
        {
            var model = dbSeguridad.DEPENDENCIA;
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
            return PartialView("_gvwAdministrarDependencia", model.ToList());
        }

        /// <summary>
        /// Actualiza una Dependencia con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos de la Dependencia</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ADEPENDENCIA")]
        public ActionResult gvwAdministrarDependenciaActualizar(DEPENDENCIA item)
        {
            var model = dbSeguridad.DEPENDENCIA;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_DEPENDENCIA == item.ID_DEPENDENCIA);
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
            return PartialView("_gvwAdministrarDependencia", model.ToList());
        }

        /// <summary>
        /// Elimina una Dependencia
        /// </summary>
        /// <param name="ID_CARGO">Identificador de la Dependencia a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EDEPENDENCIA")]
        public ActionResult gvwAdministrarDependenciaEliminar(System.Decimal ID_DEPENDENCIA)
        {
            var model = dbSeguridad.DEPENDENCIA;
            if (ID_DEPENDENCIA != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_DEPENDENCIA == ID_DEPENDENCIA);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarDependencia", model.ToList());
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
