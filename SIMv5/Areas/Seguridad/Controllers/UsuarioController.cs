using DevExpress.Web.Mvc;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador Usuario: Creación, modificación, borrado y consulta de los ssuarios del sistema
    /// </summary>
    public class UsuarioController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Método por defecto del controlador. Carga la vista para la administacion de la entidad Usuario
        /// </summary>
        /// <returns>Vista de administración de Usuario</returns>
        [Authorize(Roles = "VUSUARIO")]
        public ActionResult Index()
        {
            string actionName = RouteData.Values["action"].ToString();
            string controllerName = RouteData.Values["controller"].ToString();
            string areaName = RouteData.DataTokens["area"].ToString();
            var _IdUsuario = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("nameidentifier")).FirstOrDefault();
            decimal IdUsuario = 0;
            decimal.TryParse(_IdUsuario.Value, out IdUsuario);
            PermisosRolModel permisos = SIM.Utilidades.Security.PermisosFormulario(areaName, controllerName, actionName, IdUsuario);
            return View(permisos);
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Usuario
        /// </summary>
        /// <returns>Vista de Consulta de Usuario</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VUSUARIO")]
        public ActionResult gvwAdministrarUsuario()
        {
            var model = dbSIM.USUARIO;
            return PartialView("_gvwAdministrarUsuario", model.ToList());
        }

        /// <summary>
        /// Crea un Usuario 
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Usuario</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CUSUARIO")]
        public ActionResult gvwAdministrarUsuarioCrear([ModelBinder(typeof(DevExpressEditorsBinder))] USUARIO item)
        {
            var model = dbSIM.USUARIO;
            if (ModelState.IsValid)
            {
                try
                {
                    item.D_REGISTRO = item.D_REGISTRO == null ? System.DateTime.Now : item.D_REGISTRO;
                    model.Add(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return PartialView("_gvwAdministrarUsuario", model.ToList());
        }

        /// <summary>
        /// Actualiza un Usuario con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del Usuario</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "AUSUARIO")]
        public ActionResult gvwAdministrarUsuarioActualizar([ModelBinder(typeof(DevExpressEditorsBinder))] USUARIO item)
        {
            var model = dbSIM.USUARIO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_USUARIO == item.ID_USUARIO);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = Properties.ResourcesError.errInformacionIncorrecta;
            return PartialView("_gvwAdministrarUsuario", model.ToList());
        }

        /// <summary>
        /// Elimina un Usuario
        /// </summary>
        /// <param name="ID_CARGO">Identificador del Usuario a eliminar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "EUSUARIO")]
        public ActionResult gvwAdministrarUsuarioEliminar(System.Int32 ID_USUARIO)
        {
            var model = dbSIM.USUARIO;
            if (ID_USUARIO != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_USUARIO == ID_USUARIO);
                    if (item != null)
                        model.Remove(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarUsuario", model.ToList());
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView del Usuario para listar sus elementos
        /// </summary>
        /// <returns>Vista de Consulta del Usuario para listar sus elementos</returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult gvwSeleccionarUsuario()
        {
            bool lcbolFiltro = false;
            var model = from usuario in dbSIM.USUARIO
                        select new
                        {
                            usuario.ID_USUARIO,
                            usuario.S_LOGIN,
                            S_NOMBRE = usuario.S_NOMBRES + " " + usuario.S_APELLIDOS
                        };

            for (int lcintCont = 0; lcintCont < 2; lcintCont++)
            {
                if (Request.Params["gvwSelectUsuarios$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwSelectUsuarios$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    lcbolFiltro = true;
                    break;
                }
            }

            if (lcbolFiltro)
                return PartialView("_gvwSeleccionarUsuario", model.ToList());
            else
                return PartialView("_gvwSeleccionarUsuario", model.Where(p => p.ID_USUARIO == -10000).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbSIM.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
