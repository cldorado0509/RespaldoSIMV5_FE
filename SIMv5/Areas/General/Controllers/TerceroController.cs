using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Data.General;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador para la administración de Terceros
    /// </summary>
    public class TerceroController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso a los esquemas del SIM
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Método por defecto del controlador. Carga la vista de Consulta de Terceros
        /// </summary>
        /// <returns>Vista de Consulta de Terceros</returns>
        [Authorize(Roles = "VTERCERO")]
        public ActionResult Index(bool? popup)
        {
            //Utilidades.Email.EnviarEmail("trabajoprueba1208@gmail.com", "renemeneses1208@hotmail.com", "Validación de usuario registrados en el SIM", "Prueba Correo", "smtp.gmail.com", true, "trabajoprueba1208@gmail.com", "renemeneses");
            if (popup == null) ViewBag.popup = false;
            else ViewBag.popup = popup;
            return View();
        }





        /// <summary>
        /// Carga modelo del tercero seleccionado
        /// </summary>
        /// <param name="id">ID del Tercero Seleccionado. Si es NULL, significa que se va a crear un Tercero</param>
        /// <param name="tipoTercero">Tipo de Tercero para creación: "N" Natural, "J" Jurídico</param>
        /// <returns>Vista de Consulta de detalle de Tercero</returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Tercero(int? id, string tipoTercero, bool? vistaRetorno, bool? incrustado)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            ViewBag.OcultarMenu = incrustado;
            ViewBag.idTercero = (id == null ? 0 : id);
            int? idTerceroUsuario = null;

            var model = from roles in dbSIM.ROL.Where(r => r.S_TIPO == "E")
                        orderby roles.S_NOMBRE
                        select new ROLESUSUARIO()
                        {
                            SEL = false,
                            ID_ROL = roles.ID_ROL,
                            S_NOMBRE = roles.S_NOMBRE
                        };

            var administrador = false;

            // Es el usuario administrador?
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            // Se obtiene el Tercero asociado al usuario actual
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            if (tipoTercero != null)
            {
                // Si no es administrador no deberia asignarse el tipo tercero, por lo tanto se recarga la accion sin parámetros
                if (!administrador && id == null)
                    return RedirectToAction("Tercero", "Tercero", new { area = "General" });
            }
            else
            {
                if (id != null) // Si id es null el tercero es nuevo de lo contrario debería existir
                {
                    // Si no es administrador no puede cargar un tercero diferente al asociado al usuario actual
                    if (!administrador && idTerceroUsuario != id)
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    TERCERO tercero = dbSIM.TERCERO.Find(id);
                    tipoTercero = (tercero.ID_TIPODOCUMENTO == 2 ? "J" : "N");
                }
                else
                {
                    // Si el usuario ya tiene un tercero asignado, edita el tercero en vez de crear uno nuevo
                    if (idTerceroUsuario != null && !administrador)
                    {
                        id = idTerceroUsuario;

                        TERCERO tercero = dbSIM.TERCERO.Find(id);
                        tipoTercero = (tercero.ID_TIPODOCUMENTO == 2 ? "J" : "N");
                        ViewBag.idTercero = id;
                    }
                    else
                    {
                        if (!administrador)
                        {
                            var userId = Convert.ToInt32(User.Identity.GetUserId());

                            // Si el tercero es nuevo, verificamos en el usuario el tipo de persona y de acuerdo a esto establecemos el tipo de tercero
                            var tipoUsuario = (from usuario in dbSIM.USUARIO
                                               where usuario.ID_USUARIO == userId
                                               select new
                                               {
                                                   usuario.S_TIPO,
                                                   usuario.S_NOMBRES,
                                                   usuario.S_APELLIDOS,
                                                   usuario.S_EMAIL
                                               }
                                              ).FirstOrDefault();

                            if (tipoUsuario == null)
                                tipoTercero = "N";
                            else
                            {
                                tipoTercero = tipoUsuario.S_TIPO;

                                ViewBag.nombres = tipoUsuario.S_NOMBRES;
                                long i;
                                if (tipoTercero == "J" && long.TryParse(tipoUsuario.S_APELLIDOS, out i))
                                    ViewBag.apellidos = tipoUsuario.S_APELLIDOS;
                                ViewBag.email = tipoUsuario.S_EMAIL;
                            }
                        }
                    }
                }
            }

            ViewBag.tipoTercero = tipoTercero;
            ViewBag.vistaRetorno = (vistaRetorno == null ? "N" : ((bool)vistaRetorno ? "S" : "N"));

            if (tipoTercero == "N")
            {
                ViewBag.tiposIdentificacion = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoNatural());
                ViewBag.tiposIdentificacionNatural = ViewBag.tiposIdentificacion;
            }
            else
            {
                ViewBag.tiposIdentificacion = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoJuridica());
                ViewBag.tiposIdentificacionNatural = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoNatural());
            }

            return View(model);
        }

        /// <summary>
        /// Carga la vista de Información General del Tercero
        /// </summary>
        /// <param name="id">Id del Tercero</param>
        /// <param name="tipoTercero">Tipo Tercero (N, J) que determina los campos que debe cargar</param>
        /// <returns>Vista de Información General</returns>
        [Authorize]
        public ActionResult TerceroInformacionGeneral(int? id, string tipoTercero)
        {
            if (ValidarAccesoUsuarioDatosTercero(id))
            {
                ViewBag.idTercero = id;
                ViewBag.tipoTercero = tipoTercero;

                return PartialView();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Carga la vista de Contactos del Tercero
        /// </summary>
        /// <param name="id">Id del Tercero</param>
        /// <param name="tipoTercero">Tipo Tercero (N, J) que determina los campos que debe cargar</param>
        /// <returns>Vista de Contactos</returns>
        [Authorize]
        public ActionResult TerceroContactos(int? id, string tipoTercero)
        {
            if (ValidarAccesoUsuarioDatosTercero(id))
            {
                ViewBag.idTercero = id;
                ViewBag.tipoTercero = tipoTercero;

                return PartialView();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Carga la vista de Instalaciones del Tercero
        /// </summary>
        /// <param name="id">Id del Tercero</param>
        /// <param name="tipoTercero">Tipo Tercero (N, J) que determina los campos que debe cargar</param>
        /// <returns>Vista de Instalaciones</returns>
        [Authorize]
        public ActionResult TerceroInstalaciones(int? id, string tipoTercero)
        {
            if (ValidarAccesoUsuarioDatosTercero(id))
            {
                ViewBag.idTercero = id;
                ViewBag.tipoTercero = tipoTercero;

                return PartialView();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Carga la vista de Usuarios del Tercero
        /// </summary>
        /// <param name="id">Id del Tercero</param>
        /// <param name="tipoTercero">Tipo Tercero (N, J) que determina los campos que debe cargar</param>
        /// <returns>Vista de Usuarios</returns>
        [Authorize]
        public ActionResult TerceroUsuarios(int? id, string tipoTercero)
        {
            if (ValidarAccesoUsuarioDatosTercero(id))
            {
                var model = from roles in dbSIM.ROL.Where(r => r.S_TIPO == "E")
                            orderby roles.S_NOMBRE
                            select new ROLESUSUARIO()
                            {
                                SEL = false,
                                ID_ROL = roles.ID_ROL,
                                S_NOMBRE = roles.S_NOMBRE
                            };

                ViewBag.idTercero = id;
                ViewBag.tipoTercero = tipoTercero;

                return PartialView(model);
            }
            else
            {
                return null;
            }
        }

        private bool ValidarAccesoUsuarioDatosTercero(int? id)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            ViewBag.idTercero = (id == null ? 0 : id);
            int? idTerceroUsuario = null;

            var administrador = false;

            // Es el usuario administrador?
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            // Se obtiene el Tercero asociado al usuario actual
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            return (administrador || id == idTerceroUsuario);
        }
    }
}