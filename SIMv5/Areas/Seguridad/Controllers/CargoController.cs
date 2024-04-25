using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador Cargo: Creación, modificación, borrado y consulta de los cargos
    /// </summary>
    public class CargoController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad Cargo
        /// </summary>
        /// <returns>Vista de administración de Cargo</returns>
        [Authorize(Roles = "VCARGO")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Cargo
        /// </summary>
        /// <returns>Vista de Consulta de Cargo</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VCARGO")]
        public ActionResult gvwAdministrarCargo()
        {
            var model = dbSeguridad.CARGO;
            return PartialView("_gvwAdministrarCargo", model.ToList());
        }

        /// <summary>
        /// Crea un cargo 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del cargo</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CCARGO")]
        public ActionResult gvwAdministrarCargoCrear(CARGO item)
        {
            var model = dbSeguridad.CARGO;
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
            return PartialView("_gvwAdministrarCargo", model.ToList());
        }

        /// <summary>
        /// Actualiza un cargo con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del cargo</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ACARGO")]
        public ActionResult gvwAdministrarCargoActualizar(CARGO item)
        {
            var model = dbSeguridad.CARGO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_CARGO == item.ID_CARGO);
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
            return PartialView("_gvwAdministrarCargo", model.ToList());
        }

        /// <summary>
        /// Elimina un cargo
        /// </summary>
        /// <param name="ID_CARGO">Identificador del cargo a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ECARGO")]
        public ActionResult gvwAdministrarCargoEliminar(System.Int32 ID_CARGO)
        {
            var model = dbSeguridad.CARGO;
            if (ID_CARGO != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_CARGO == ID_CARGO);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarCargo", model.ToList());
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
