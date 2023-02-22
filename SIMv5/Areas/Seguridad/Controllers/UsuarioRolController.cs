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
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using SIM.Data.Seguridad;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador UsuarioRol: Creación, modificación, borrado y consulta de los registros asociados a la relacion de los roles de un usuario
    /// </summary>
    public class UsuarioRolController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad UsuarioRol
        /// </summary>
        /// <returns>Vista de administración de UsuarioRol</returns>
        [Authorize(Roles = "VUSUARIOROL")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de UsuarioRol
        /// </summary>
        /// <returns>Vista de Consulta de UsuarioRol</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VUSUARIOROL")]
        public ActionResult gvwAdministrarUsuarioRol()
        {
            var model = dbSeguridad.USUARIO_ROL;
            return PartialView("_gvwAdministrarUsuarioRol", model.ToList());
        }

        /// <summary>
        /// Crea un UsuarioRol 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del UsuarioRol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CUSUARIOROL")]
        public ActionResult gvwAdministrarUsuarioRolCrear(USUARIO_ROL item)
        {
            var model = dbSeguridad.USUARIO_ROL;
            int idusuario = string.IsNullOrEmpty(Request.Params["tbhIdUsuario"]) ? -1 : int.Parse(Request.Params["tbhIdUsuario"]);
            string[] idroles = Request.Params["tbhIdRoles"].Substring(0, Request.Params["tbhIdRoles"].Length - 1).Split(';');

            if (ModelState.IsValid && idroles.Length > 0 && idusuario > 0)
            {
                try
                {
                    foreach (string r in idroles)
                    {
                        int rol = int.Parse(r);
                        if (model.Where(td => td.ID_USUARIO == idusuario && td.ID_ROL == rol).Count() == 0)
                        {
                            item = new USUARIO_ROL();
                            item.ID_USUARIO = idusuario;
                            item.ID_ROL = rol;
                            model.Add(item);
                        }
                    }
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return View("Index");
        }

        /// <summary>
        /// Actualiza un UsuarioRol con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del UsuarioRol</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AUSUARIOROL")]
        public ActionResult gvwAdministrarUsuarioRolActualizar(USUARIO_ROL item)
        {
            var model = dbSeguridad.USUARIO_ROL;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_USUARIO_ROL == item.ID_USUARIO_ROL);
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
            return PartialView("_gvwAdministrarUsuarioRol", model.ToList());
        }

        /// <summary>
        /// Elimina un UsuarioRol
        /// </summary>
        /// <param name="ID_CARGO">Identificador del UsuarioRol a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EUSUARIOROL")]
        public ActionResult gvwAdministrarUsuarioRolEliminar(System.Int32 ID_USUARIO_ROL)
        {
            var model = dbSeguridad.USUARIO_ROL;
            if (ID_USUARIO_ROL != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_USUARIO_ROL == ID_USUARIO_ROL);
                    if (item != null)
                        model.Remove(item);
                    dbSeguridad.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarUsuarioRol", model.ToList());
        }

        [Authorize]
        public ActionResult AsignarRolUsuarioExterno()
        {
            int idUsuario = Convert.ToInt32(User.Identity.GetUserId());

            var model = from roles in dbSeguridad.ROL.Where(r => r.S_TIPO == "E")
                        join usuario in dbSeguridad.USUARIO_ROL.Where(u => u.ID_USUARIO == idUsuario) on roles.ID_ROL equals usuario.ID_ROL into rolesUsuario
                        from usuario in rolesUsuario.DefaultIfEmpty()
                        orderby roles.S_NOMBRE
                        select new ROLESUSUARIO()
                        {
                            SEL = usuario == null ? false : true,
                            ID_ROL = roles.ID_ROL,
                            S_NOMBRE = roles.S_NOMBRE
                        };

            return View("RolUsuarioExterno", model.ToList());
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
