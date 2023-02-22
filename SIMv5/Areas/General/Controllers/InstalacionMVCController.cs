using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.General.Models;
using SIM.Areas.Tramites.Models;
using SIM.Areas.Residuo.Models;
using SIM.Areas.Models;
using System.Data;
using SIM.Areas.Seguridad.Models;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador Instalacion: Creación, modificación, borrado y consulta de Instalaciones
    /// </summary>
    public class InstalacionMVCController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Método por defecto del controlador. Carga la vista de Consulta de Instalaciones
        /// </summary>
        /// <returns>Vista de Consulta de Instalaciones</returns>
        //[Authorize(Roles = "VINSTALACION")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Instalaciones de acuerdo a los filtros seleccionados
        /// </summary>
        /// <returns>Vista de Consulta de Terceros</returns>
        [ValidateInput(false)]
        //[Authorize(Roles = "VINSTALACION")]
        public ActionResult gvwAdministrarInstalacion()
        {
            int? idTercero = null;
            int? idRol = null;
            bool lcbolFiltro = false;
            var model = dbSIM.INSTALACION;

            // Restricción solo para el perfil declarante
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null && int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value) == 3)
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                }
                else
                {
                    idTercero = -10000;
                }
            }

            ViewBag.Administrador = false;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                ViewBag.Administrador = (idRol == 24);
            }

            for (int lcintCont = 0; lcintCont < 20; lcintCont++)
            {
                if (Request.Params["gvwInstalacion$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwInstalacion$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    lcbolFiltro = true;
                    break;
                }
            }

            if (idTercero == null)
            {
                var qryInstalaciones = from instalacion in dbSIM.INSTALACION
                                       join terceroinstalacion in dbSIM.TERCERO_INSTALACION on instalacion.ID_INSTALACION equals terceroinstalacion.ID_INSTALACION
                                       join tipoinstalacion in dbSIM.TIPO_INSTALACION on terceroinstalacion.ID_TIPOINSTALACION equals tipoinstalacion.ID_TIPOINSTALACION
                                       join actividadeconomica in dbSIM.ACTIVIDAD_ECONOMICA on terceroinstalacion.ID_ACTIVIDADECONOMICA equals actividadeconomica.ID_ACTIVIDADECONOMICA into aej
                                       from instalaciones in aej.DefaultIfEmpty()
                                       join estado in dbSIM.ESTADO on terceroinstalacion.ID_ESTADO equals estado.ID_ESTADO into ej
                                       from estados in ej.DefaultIfEmpty()
                                       select new
                                       {
                                           instalacion.ID_INSTALACION,
                                           S_INSTALACION = instalacion.S_NOMBRE,
                                           S_TIPOINSTALACION = tipoinstalacion.S_NOMBRE,
                                           S_ACTIVIDADECONOMICA = instalaciones.S_NOMBRE,
                                           instalacion.S_TELEFONO,
                                           terceroinstalacion.ID_TERCERO,
                                           terceroinstalacion.D_INICIO,
                                           terceroinstalacion.D_FIN,
                                           S_ESTADO = estados.S_NOMBRE
                                       };
                if (lcbolFiltro || !ViewBag.Administrador)
                    return PartialView("_gvwAdministrarInstalacion", qryInstalaciones);
                else
                    return PartialView("_gvwAdministrarInstalacion", qryInstalaciones.Where(p => p.ID_INSTALACION == -10000));
            }
            else
            {
                var qryInstalaciones = from instalacion in dbSIM.INSTALACION
                                       join terceroinstalacion in dbSIM.TERCERO_INSTALACION on instalacion.ID_INSTALACION equals terceroinstalacion.ID_INSTALACION
                                       join tipoinstalacion in dbSIM.TIPO_INSTALACION on terceroinstalacion.ID_TIPOINSTALACION equals tipoinstalacion.ID_TIPOINSTALACION
                                       join actividadeconomica in dbSIM.ACTIVIDAD_ECONOMICA on terceroinstalacion.ID_ACTIVIDADECONOMICA equals actividadeconomica.ID_ACTIVIDADECONOMICA into aej
                                       from instalaciones in aej.DefaultIfEmpty()
                                       join estado in dbSIM.ESTADO on terceroinstalacion.ID_ESTADO equals estado.ID_ESTADO into ej
                                       from estados in ej.DefaultIfEmpty()
                                       where terceroinstalacion.ID_TERCERO == idTercero
                                       select new
                                       {
                                           instalacion.ID_INSTALACION,
                                           S_INSTALACION = instalacion.S_NOMBRE,
                                           S_TIPOINSTALACION = tipoinstalacion.S_NOMBRE,
                                           S_ACTIVIDADECONOMICA = instalaciones.S_NOMBRE,
                                           instalacion.S_TELEFONO,
                                           terceroinstalacion.ID_TERCERO,
                                           terceroinstalacion.D_INICIO,
                                           terceroinstalacion.D_FIN,
                                           S_ESTADO = estados.S_NOMBRE
                                       };
                if (lcbolFiltro || !ViewBag.Administrador)
                    return PartialView("_gvwAdministrarInstalacion", qryInstalaciones);
                else
                    return PartialView("_gvwAdministrarInstalacion", qryInstalaciones.Where(p => p.ID_INSTALACION == -10000));
            }
        }

        /// <summary>
        /// Carga modelo de la Instalación seleccionada
        /// </summary>
        /// <param name="id">ID de la Instalación Seleccionada. Si es NULL, significa que se va a crear una instalación</param>
        /// <returns>Vista de Consulta de detalle de la Instalación</returns>
        [ValidateInput(false)]
        //[Authorize(Roles = "VINSTALACION")]
        public ActionResult LoadInstalacion(int? id, int? idTerceroAsociado)
        {
            INSTALACION instalacion;
            int idRol = -1;

            ViewBag.Administrador = false;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                ViewBag.Administrador = (idRol == 24);
            }

            if (id == null) // Si id es null es una nueva instalación
            {
                instalacion = new INSTALACION();
                instalacion.ID_INSTALACION = -1000;
                instalacion.ID_ESTADO = 1;

                // Si el usuario es declarante y crea una instalación, el tercero debe ser obligatoriamente el tercero del usuario autenticado
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    var idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                    if (idRol == 3)
                    {
                        TERCERO tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == idTercero).FirstOrDefault();
                        if (tercero != null)
                        {
                            ViewBag.Tercero = tercero.N_DOCUMENTON.ToString() + " - " + tercero.S_RSOCIAL;
                            ViewBag.IdTercero = tercero.ID_TERCERO;
                            instalacion.S_NOMBRE = tercero.S_RSOCIAL;
                        }

                        /*
                        ACTIVIDAD_ECONOMICA actividadEconomica = tercero.ACTIVIDAD_ECONOMICA;
                        if (actividadEconomica != null)
                        {
                            ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
                            ViewBag.IdActividadEconomica = actividadEconomica.ID_ACTIVIDADECONOMICA;
                        }*/

                        ViewBag.IdTercero = idTercero;
                    }
                    else
                    {
                        if (idTerceroAsociado != null)
                        {
                            TERCERO tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == idTerceroAsociado).FirstOrDefault();
                            if (tercero != null)
                            {
                                instalacion.S_NOMBRE = tercero.S_RSOCIAL;
                            }
                        }
                    }
                }
                else
                {
                    if (idTerceroAsociado != null)
                        ViewBag.IdTercero = idTerceroAsociado;
                }
            }
            else // Si id NO es null se carga el modelo de la instalación
            {
                TERCERO_INSTALACION terceroInstalacion;
                instalacion = dbSIM.INSTALACION.Find(id);

                terceroInstalacion = instalacion.TERCERO_INSTALACION.Where(c => c.D_FIN == null).FirstOrDefault();
                if (terceroInstalacion != null)
                {
                    ACTIVIDAD_ECONOMICA actividadEconomica = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICA == terceroInstalacion.ID_ACTIVIDADECONOMICA).FirstOrDefault();
                    if (actividadEconomica != null)
                    {
                        ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
                        ViewBag.IdActividadEconomica = actividadEconomica.ID_ACTIVIDADECONOMICA;
                    }

                    TERCERO tercero = terceroInstalacion.TERCERO;
                    if (tercero != null)
                    {
                        ViewBag.Tercero = tercero.N_DOCUMENTON.ToString() + " - " + tercero.S_RSOCIAL;
                        ViewBag.IdTercero = tercero.ID_TERCERO;
                    }

                    TBPROYECTO proyecto = dbSIM.TBPROYECTO.Where(proy => proy.CODIGO_PROYECTO == terceroInstalacion.CODIGO_PROYECTO).FirstOrDefault();
                    if (proyecto != null)
                    {
                        ViewBag.Proyecto = proyecto.CM + " - " + proyecto.NOMBRE;
                        ViewBag.IdProyecto = proyecto.CODIGO_PROYECTO;
                    }

                    ESTADO estado = dbSIM.ESTADO.Where(est => est.ID_ESTADO == terceroInstalacion.ID_ESTADO).FirstOrDefault();
                    if (estado != null)
                    {
                        ViewBag.Estado = estado.S_NOMBRE;
                        ViewBag.IdEstado = estado.ID_ESTADO;
                    }

                    if (terceroInstalacion.ID_TIPOINSTALACION != null)
                        ViewBag.IdTipoInstalacion = terceroInstalacion.ID_TIPOINSTALACION;

                    TERCERO_TIPO_DECLARACION terceroTipoDeclaracion = dbSIM.TERCERO_TIPO_DECLARACION.Where(c => c.ID_INSTALACION == terceroInstalacion.ID_INSTALACION && c.ID_TERCERO == terceroInstalacion.ID_TERCERO && c.D_FIN == null).FirstOrDefault();
                    if (terceroTipoDeclaracion != null)
                    {
                        ViewBag.IdTipoDeclaracion = terceroTipoDeclaracion.ID_TIPODECLARACION;
                    }

                    if (terceroInstalacion.D_INICIO != null)
                        ViewBag.FechaInicio = terceroInstalacion.D_INICIO;
                    if (terceroInstalacion.D_FIN != null)
                        ViewBag.FechaFin = terceroInstalacion.D_FIN;
                }
            }
            return View("Instalacion", instalacion);
        }

        /// <summary>
        /// Actualiza una Instalación con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos de la Instalación</param>
        /// <returns></returns>
        [ValidateInput(false)]
        //[Authorize(Roles = "AINSTALACION")]
        public ActionResult InstalacionUpdate(INSTALACION item)
        {
            var model = dbSIM.INSTALACION;

            item.ID_ESTADO = 1;
            if (ModelState.IsValid)
            {
                try
                {
                    // Busca si la instalación existe
                    var modelItem = model.FirstOrDefault(it => it.ID_INSTALACION == item.ID_INSTALACION);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);

                        modelItem.DIVIPOLA = dbSIM.DIVIPOLA.FirstOrDefault(c => c.ID_DIVIPOLA == modelItem.ID_DIVIPOLA);

                        // Carga los datos del Tercero-Instalación si existe, de lo contrario crea un registro
                        TERCERO_INSTALACION terceroInstalacion = modelItem.TERCERO_INSTALACION.Where(c => c.D_FIN == null).FirstOrDefault();

                        if (terceroInstalacion == null)
                        {
                            modelItem.TERCERO_INSTALACION.Add(new TERCERO_INSTALACION());
                            terceroInstalacion = modelItem.TERCERO_INSTALACION.Where(c => c.D_FIN == null).First();
                        }

                        terceroInstalacion.ID_INSTALACION = item.ID_INSTALACION;
                        terceroInstalacion.ID_TERCERO = Convert.ToInt32(Request.Params["ID_PROPIETARIO"]);
                        terceroInstalacion.ID_TIPOINSTALACION = Convert.ToInt32(Request.Params["ID_TIPOINSTALACION_VI"]);
                        terceroInstalacion.ID_ACTIVIDADECONOMICA = Convert.ToInt32(Request.Params["ID_ACTIVIDADECONOMICA"]);
                        if (Request.Params["CODIGO_PROYECTO"] != "")
                            terceroInstalacion.CODIGO_PROYECTO = Convert.ToInt32(Request.Params["CODIGO_PROYECTO"]);
                        terceroInstalacion.ID_ESTADO = Convert.ToInt32(Request.Params["ID_ESTADO_IT_VI"]);
                        terceroInstalacion.D_REGISTRO = DateTime.Today;

                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        item.DIVIPOLA = dbSIM.DIVIPOLA.FirstOrDefault(c => c.ID_DIVIPOLA == item.ID_DIVIPOLA);
                        model.Add(item);

                        dbSIM.SaveChanges();

                        TERCERO_INSTALACION terceroInstalacion = new TERCERO_INSTALACION();//;item.TERCERO_INSTALACION.Where(c => c.D_FIN == null).FirstOrDefault();

                        terceroInstalacion.ID_INSTALACION = item.ID_INSTALACION;
                        terceroInstalacion.ID_TERCERO = Convert.ToInt32(Request.Params["ID_PROPIETARIO"]);
                        terceroInstalacion.ID_TIPOINSTALACION = Convert.ToInt32(Request.Params["ID_TIPOINSTALACION_VI"]);
                        terceroInstalacion.ID_ACTIVIDADECONOMICA = Convert.ToInt32(Request.Params["ID_ACTIVIDADECONOMICA"]);
                        if (Request.Params["CODIGO_PROYECTO"] != "")
                            terceroInstalacion.CODIGO_PROYECTO = Convert.ToInt32(Request.Params["CODIGO_PROYECTO"]);
                        terceroInstalacion.ID_ESTADO = Convert.ToInt32(Request.Params["ID_ESTADO_IT_VI"]);
                        terceroInstalacion.D_REGISTRO = DateTime.Today;
                        terceroInstalacion.S_ACTUAL = "1";

                        dbSIM.Entry(terceroInstalacion).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    bool crearTipoDeclaracion = false;
                    int idTercero = Convert.ToInt32(Request.Params["ID_PROPIETARIO"]);

                    TERCERO_TIPO_DECLARACION terceroTipoDeclaracion = dbSIM.TERCERO_TIPO_DECLARACION.Where(td => td.ID_TERCERO == idTercero && td.ID_INSTALACION == item.ID_INSTALACION).FirstOrDefault();

                    if (terceroTipoDeclaracion != null)
                    {
                        if (terceroTipoDeclaracion.ID_TIPODECLARACION != Convert.ToInt32(Request.Params["ID_TIPODECLARACION_VI"]))
                        {
                            dbSIM.Entry(terceroTipoDeclaracion).State = EntityState.Deleted;
                            dbSIM.SaveChanges();

                            crearTipoDeclaracion = true;
                        }
                    }
                    else
                    {
                        crearTipoDeclaracion = true;
                    }

                    if (crearTipoDeclaracion)
                    {
                        terceroTipoDeclaracion = new TERCERO_TIPO_DECLARACION { ID_TERCERO = Convert.ToInt32(Request.Params["ID_PROPIETARIO"]), ID_INSTALACION = item.ID_INSTALACION, ID_TIPODECLARACION = Convert.ToInt32(Request.Params["ID_TIPODECLARACION_VI"]), D_INICIO = DateTime.Today };

                        dbSIM.Entry(terceroTipoDeclaracion).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["ErrorMessage"] = e.Message;
                }
            }
            else
                ViewData["ErrorMessage"] = "Please, correct all errors.";


            if (ViewData["ErrorMessage"] == null)
            {
                // Después de actualizar el tercero se carga con los datos actualizados
                return RedirectToAction("LoadInstalacion", new { id = item.ID_INSTALACION });
            }
            else
            {
                /*
                if (item.TERCERO_INSTALACION)
                ACTIVIDAD_ECONOMICA actividadEconomica = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICA == terceroInstalacion.ID_ACTIVIDADECONOMICA).FirstOrDefault();
                ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
                ViewBag.IdActividadEconomica = actividadEconomica.ID_ACTIVIDADECONOMICA;

                TERCERO tercero = terceroInstalacion.TERCERO;
                ViewBag.Tercero = tercero.N_DOCUMENTON.ToString() + " - " + tercero.S_RSOCIAL;
                ViewBag.IdTercero = tercero.ID_TERCERO;

                TBPROYECTO proyecto = dbSIM.TBPROYECTO.Where(proy => proy.CODIGO_PROYECTO == terceroInstalacion.CODIGO_PROYECTO).FirstOrDefault();
                if (proyecto != null)
                {
                    ViewBag.Proyecto = proyecto.CM + " - " + proyecto.NOMBRE;
                    ViewBag.IdProyecto = proyecto.CODIGO_PROYECTO;
                }

                ESTADO estado = dbSIM.ESTADO.Where(est => est.ID_ESTADO == terceroInstalacion.ID_ESTADO).FirstOrDefault();
                ViewBag.Estado = estado.S_NOMBRE;
                ViewBag.IdEstado = estado.ID_ESTADO;

                ViewBag.IdTipoInstalacion = terceroInstalacion.ID_TIPOINSTALACION;

                TERCERO_TIPO_DECLARACION terceroTipoDeclaracion = dbSIM.TERCERO_TIPO_DECLARACION.Where(c => c.ID_INSTALACION == terceroInstalacion.ID_INSTALACION && c.ID_TERCERO == terceroInstalacion.ID_TERCERO && c.D_FIN == null).FirstOrDefault();
                ViewBag.IdTipoDeclaracion = terceroTipoDeclaracion.ID_TIPODECLARACION;

                ViewBag.FechaInicio = terceroInstalacion.D_INICIO;
                ViewBag.FechaFin = terceroInstalacion.D_FIN;

                if (item.ID_ACTIVIDADECONOMICA != null)
                {
                    ACTIVIDAD_ECONOMICA actividadEconomica = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICA == item.ID_ACTIVIDADECONOMICA).First();
                    ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
                }

                if (item.ID_TIPODOCUMENTO == 2)
                    ViewBag.tipoTercero = "J";
                else
                {
                    ViewBag.tipoTercero = "N";

                    nombres = Request.Params["txtNombres"].Split(' ');
                    apellidos = Request.Params["txtApellidos"].Split(' ');

                    item.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                    item.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                    item.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                    item.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                    item.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];
                }*/

                return View("Instalacion", item);
            }
        }

        /// <summary>
        /// Elimina el tercero suministrado
        /// </summary>
        /// <param name="ID_TERCERO">Id del tercero que se va a borrar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        //[Authorize(Roles = "ETERCERO")]
        public ActionResult InstalacionDelete(System.Int32 ID_INSTALACION)
        {
            var model = dbSIM.INSTALACION;

            try
            {
                var item = model.FirstOrDefault(i => i.ID_INSTALACION == ID_INSTALACION);
                if (item != null)
                {
                    // Se verifica que no tenga declaraciones relacionadas

                    var declaraciones = from declaracion in dbSIM.DECLARACION
                                        where declaracion.ID_INSTALACION == ID_INSTALACION
                                        select declaracion;

                    if (declaraciones.ToList().Count == 0)
                    {

                        // Se elimina el TerceroInstalacion relacionado
                        var terceroInstalacion = (from terceroinst in dbSIM.TERCERO_INSTALACION
                                                 where terceroinst.ID_INSTALACION == item.ID_INSTALACION
                                                 select terceroinst).FirstOrDefault();

                        dbSIM.Entry(terceroInstalacion).State = EntityState.Deleted;

                        // Se elimina el TipoDeclaracion relacionado
                        var tipoDeclaracion = from tipo in dbSIM.TERCERO_TIPO_DECLARACION
                                              where tipo.ID_INSTALACION == item.ID_INSTALACION
                                              select tipo;

                        tipoDeclaracion.ToList().ForEach(td =>
                        {
                            dbSIM.Entry(td).State = EntityState.Deleted;
                        });

                        model.Remove(item);
                        dbSIM.SaveChanges();
                    }
                    else
                        ViewData["EditError"] = "La Instalación no puede ser borrada, debido a que tiene declaraciones relacionadas.";
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return gvwAdministrarInstalacion();
        }

        public ActionResult LinksInstalacion()
        {
            int idTercero;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                var instalacionTercero = from instalacion in dbSIM.TERCERO_INSTALACION
                                         join actividadEconomica in dbSIM.ACTIVIDAD_ECONOMICA on instalacion.ID_ACTIVIDADECONOMICA equals actividadEconomica.ID_ACTIVIDADECONOMICA
                                         where instalacion.ID_TERCERO == idTercero
                                         select new { instalacion, actividadEconomica };

                if (instalacionTercero.Count() == 0)
                    return PartialView("_LinksInstalacion", null);
                else
                    return PartialView("_LinksInstalacion", instalacionTercero.ToList());
            }
            else
            {
                return null;
            }
        }
    }
}
