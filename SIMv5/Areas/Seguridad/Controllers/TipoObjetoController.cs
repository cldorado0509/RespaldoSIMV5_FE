using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador TipoObjeto: Creación, modificación, borrado y consulta de los tipos de objetos
    /// </summary>
    public class TipoObjetoController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad TipoObjeto
        /// </summary>
        /// <returns>Vista de administración de TipoObjeto</returns>
        [Authorize(Roles = "VTIPOOBJETO")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de TipoObjeto
        /// </summary>
        /// <returns>Vista de Consulta de TipoObjeto</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VTIPOOBJETO")]
        public ActionResult gvwAdministrarTipoObjeto()
        {
            var model = dbSeguridad.TIPO_OBJETO;
            return PartialView("_gvwAdministrarTipoObjeto", model.ToList());
        }

        /// <summary>
        /// Crea un TipoObjeto 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del TipoObjeto</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CTIPOOBJETO")]
        public ActionResult gvwAdministrarTipoObjetoCrear(TIPO_OBJETO item)
        {
            var model = dbSeguridad.TIPO_OBJETO;
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
            return PartialView("_gvwAdministrarTipoObjeto", model.ToList());
        }

        /// <summary>
        /// Actualiza un TipoObjeto con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del TipoObjeto</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ATIPOOBJETO")]
        public ActionResult gvwAdministrarTipoObjetoActualizar(TIPO_OBJETO item)
        {
            var model = dbSeguridad.TIPO_OBJETO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_TIPOOBJETO == item.ID_TIPOOBJETO);
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
            return PartialView("_gvwAdministrarTipoObjeto", model.ToList());
        }

        /// <summary>
        /// Elimina un TipoObjeto
        /// </summary>
        /// <param name="ID_CARGO">Identificador del TipoObjeto a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ETIPOOBJETO")]
        public ActionResult gvwAdministrarTipoObjetoEliminar(System.Int32 ID_TIPOOBJETO)
        {
            var model = dbSeguridad.TIPO_OBJETO;
            if (ID_TIPOOBJETO != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_TIPOOBJETO == ID_TIPOOBJETO);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarTipoObjeto", model.ToList());
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
