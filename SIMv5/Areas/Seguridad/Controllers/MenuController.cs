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
    /// Controlador Menu: Creación, modificación, borrado y consulta de items que componen el menu de la aplicacion
    /// </summary>
    public class MenuController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad Menu
        /// </summary>
        /// <returns>Vista de administración de Menu</returns>
        [Authorize(Roles = "VMENU")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView del Menu
        /// </summary>
        /// <returns>Vista de Consulta del Menu</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VMENU")]
        public ActionResult gvwAdministrarMenu()
        {
            var model = dbSeguridad.MENU;
            return PartialView("_gvwAdministrarMenu", model.OrderBy(td => td.ID_PADRE).ThenBy(td => td.ORDEN).ToList());
        }

        /// <summary>
        /// Crea un item del Menu 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del nuevo item del Menu</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CMENU")]
        public ActionResult gvwAdministrarMenuCrear([ModelBinder(typeof(DevExpressEditorsBinder))] MENU item)
        {
            var model = dbSeguridad.MENU;
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
            return PartialView("_gvwAdministrarMenu", model.OrderBy(td => td.ID_PADRE).ThenBy(td => td.ORDEN).ToList());
        }

        /// <summary>
        /// Actualiza el Menu con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Menu</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AMENU")]
        public ActionResult gvwAdministrarMenuActualizar(MENU item)
        {
            var model = dbSeguridad.MENU;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_FORMA == item.ID_FORMA);
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
            return PartialView("_gvwAdministrarMenu", model.OrderBy(td => td.ID_PADRE).ThenBy(td => td.ORDEN).ToList());
        }

        /// <summary>
        /// Elimina un item del Menu
        /// </summary>
        /// <param name="ID_CARGO">Identificador del item del Menu a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EMENU")]
        public ActionResult gvwAdministrarMenuEliminar(System.Int32 ID_FORMA)
        {
            var model = dbSeguridad.MENU;
            if (ID_FORMA != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_FORMA == ID_FORMA);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarMenu", model.ToList());
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView del Menu para listar sus elementos
        /// </summary>
        /// <returns>Vista de Consulta del Menu para listar sus elementos</returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult gvwSeleccionarFormaMenu()
        {
            var model = from m1 in dbSeguridad.MENU
                        join m2 in dbSeguridad.MENU.AsEnumerable() on m1.ID_PADRE equals m2.ID_FORMA 
                        select new
                        {
                            m1.ID_FORMA,
                            m1.S_NOMBRE,
                            m1.S_VISIBLE_MENU,
                            S_PADRE = m2.S_NOMBRE + " (" + m2.ID_FORMA.ToString() + ")"
                        };


            return PartialView("_gvwSeleccionarFormaMenu", model.ToList());
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
