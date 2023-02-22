using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Models;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Data;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador Tercero: Creación, modificación, borrado y consulta de Terceros
    /// </summary>
    public class TerceroMVCController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;

        public ActionResult PruebaDXtreme()
        {
            return View();
        }

        public string TerceroWebAPI()
        {
            var model = from p in dbSIM.PROFESION
                        select new { p.S_NOMBRE, p.S_DESCRIPCION, p.S_ACTIVO };

            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Método por defecto del controlador. Carga la vista de Consulta de Terceros
        /// </summary>
        /// <returns>Vista de Consulta de Terceros</returns>
        [Authorize(Roles = "VTERCERO")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Terceros de acuerdo a los filtros seleccionados
        /// </summary>
        /// <returns>Vista de Consulta de Terceros</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VTERCERO")]
        public ActionResult gvwAdministrarTercero()
        {
            bool tieneFiltro = false;
            var model = dbSIM.TERCERO;

            // Verifica si se estableció algún filtro al gridview
            for (int lcintCont = 0; lcintCont < 20; lcintCont++)
            {
                if (Request.Params["gvwTercero$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwTercero$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    tieneFiltro = true;
                    break;
                }
            }

            if (tieneFiltro) // Si el gridview tiene filtro, se carga la vista con el modelo Terceros completo
                return PartialView("_gvwAdministrarTercero", model);
            else // Si el gridview NO tiene filtro, se carga la vista con un registro inexistente del modelo Terceros para que se cargue el grid vacio si no hay filtros
                return PartialView("_gvwAdministrarTercero", model.Where(p => p.ID_TERCERO == -10000));
        }

        /// <summary>
        /// Consulta y carga Vista con el GridView de Terceros para Combos de acuerdo a los filtros seleccionados
        /// </summary>
        /// <returns>Vista de Consulta de Terceros</returns>
        [ValidateInput(false)]
        public ActionResult gvwSeleccionarTercero()
        {
            bool tieneFiltro = false;
            var model = dbSIM.TERCERO;

            // Verifica si se estableció algún filtro al gridview
            for (int lcintCont = 0; lcintCont < 20; lcintCont++)
            {
                if (Request.Params["gvwTerceros$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwTerceros$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    tieneFiltro = true;
                    break;
                }
            }

            if (tieneFiltro) // Si el gridview tiene filtro, se carga la vista con el modelo Terceros completo
                return PartialView("_gvwSeleccionarTercero", model);
            else // Si el gridview NO tiene filtro, se carga la vista con un registro inexistente del modelo Terceros para que se cargue el grid vacio si no hay filtros
                return PartialView("_gvwSeleccionarTercero", model.Where(p => p.ID_TERCERO == -10000));
        }

        /// <summary>
        /// Consulta y carga Vista de las Instalaciones relacionadas a un Tercero
        /// </summary>
        /// <param name="terceroID">ID del Tercero del cual se consultan las Instalaciones</param>
        /// <returns>Vista de Consulta de Instalaciones por Tercero</returns>
        [ValidateInput(false)]
        public ActionResult gvwAdministrarInstalacionesTercero(int terceroID)
        {
            // Consulta instalaciones relacionadas a un tercero
            var instalacionesTercero = from instalacion in dbSIM.INSTALACION
                                   join tercero in dbSIM.TERCERO_INSTALACION on instalacion.ID_INSTALACION equals tercero.ID_INSTALACION
                                   where tercero.ID_TERCERO == terceroID
                                   join tipoinstalacion in dbSIM.TIPO_INSTALACION on tercero.ID_TIPOINSTALACION equals tipoinstalacion.ID_TIPOINSTALACION
                                   join actividadeconomica in dbSIM.ACTIVIDAD_ECONOMICA on tercero.ID_ACTIVIDADECONOMICA equals actividadeconomica.ID_ACTIVIDADECONOMICA into aej
                                   from instalaciones in aej.DefaultIfEmpty()
                                   join estado in dbSIM.ESTADO on tercero.ID_ESTADO equals estado.ID_ESTADO into ej
                                   from estados in ej.DefaultIfEmpty()
                                   select new
                                   {
                                       instalacion.ID_INSTALACION,
                                       S_INSTALACION = instalacion.S_NOMBRE,
                                       S_TIPOINSTALACION = tipoinstalacion.S_NOMBRE,
                                       S_ACTIVIDADECONOMICA = instalaciones.S_NOMBRE,
                                       instalacion.S_TELEFONO,
                                       tercero.ID_TERCERO,
                                       tercero.D_INICIO,
                                       tercero.D_FIN,
                                       S_ESTADO = estados.S_NOMBRE
                                   };

            ViewBag.terceroID = terceroID;

            return PartialView("_gvwAdministrarInstalacionesTercero", instalacionesTercero.ToList());
        }

        /// <summary>
        /// Carga modelo del tercero seleccionado
        /// </summary>
        /// <param name="id">ID del Tercero Seleccionado. Si es NULL, significa que se va a crear un Tercero</param>
        /// <param name="tipoTercero">Tipo de Tercero para creación: "N" Natural, "J" Jurídico</param>
        /// <returns>Vista de Consulta de detalle de Tercero</returns>
        [ValidateInput(false)]
        [Authorize(Roles = "VTERCERO")]
        public ActionResult LoadTercero(int? id, string tipoTercero)
        {
            TERCERO tercero;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null && int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value) == 3)
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    id = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                }
            }

            if (id == null) // Si id es null es un nuevo tercero y se establece el tipo de tercero
            {
                if (tipoTercero == null)
                {
                    return View("SeleccionarTipoTercero");
                }
                else
                {
                    tipoTercero = tipoTercero.Substring(0, 1);
                    tercero = new TERCERO();
                    tercero.ID_TERCERO = -1000;
                    if (tipoTercero == "N")
                        tercero.ID_TIPODOCUMENTO = 1;
                    else
                        tercero.ID_TIPODOCUMENTO = 2;

                    ViewBag.tipoTercero = tipoTercero;
                }
            }
            else // Si id NO es null se carga el modelo del tercero y se establece consulta el tipo de tercero relacionado
            {
                tercero = dbSIM.TERCERO.Find(id);

                if (tercero.ID_ACTIVIDADECONOMICA != null)
                {
                    ACTIVIDAD_ECONOMICA actividadEconomica = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICA == tercero.ID_ACTIVIDADECONOMICA).First();
                    ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
                }
                ViewBag.tipoTercero = (tercero.ID_TIPODOCUMENTO == 2 ? "J" : "N");
            }
            return View("Tercero", tercero);
        }

        [ValidateInput(false)]
        public ActionResult frmAdministrarTerceroDGA(int tipodoc, string documento, int idDGA)
        {
            PERSONAL_DGA personaDGA;

            long documentoN = long.Parse(documento);

            personaDGA = (from persona in dbSIM.PERSONAL_DGA
                             where persona.TERCERO.ID_TIPODOCUMENTO == tipodoc && persona.TERCERO.N_DOCUMENTON == documentoN
                             select persona).FirstOrDefault();

            if (personaDGA == null)
            {
                var terceroDGA = (from tercero in dbSIM.TERCERO
                             where tercero.ID_TIPODOCUMENTO == tipodoc && tercero.N_DOCUMENTON == documentoN
                             select tercero).FirstOrDefault();

                personaDGA = new PERSONAL_DGA();
                personaDGA.ID_DGA = idDGA;
                if (terceroDGA != null)
                {
                    personaDGA.TERCERO = terceroDGA;
                }
                else
                {
                    personaDGA.TERCERO = new TERCERO();
                    personaDGA.TERCERO.NATURAL = new NATURAL();
                    personaDGA.TERCERO.ID_TIPODOCUMENTO = tipodoc;
                    personaDGA.TERCERO.N_DOCUMENTON = documentoN;
                    personaDGA.TERCERO.N_DIGITOVER = (short?)ObtenerDigitoVerificacion(documento);
                    personaDGA.TERCERO.NATURAL.S_NOMBRE1 = "";
                    personaDGA.TERCERO.NATURAL.S_NOMBRE2 = "";
                    personaDGA.TERCERO.NATURAL.S_APELLIDO1 = "";
                    personaDGA.TERCERO.NATURAL.S_APELLIDO2 = "";
                }
            }

            return LoadTerceroDGADatos(personaDGA);
        }

        public ActionResult LoadTerceroDGA(int? id, int idDGA)
        {
            PERSONAL_DGA terceroDGA;

            if (id == null) // Si id es null es un nuevo tercero
            {
                terceroDGA = new PERSONAL_DGA();
                terceroDGA.ID_DGA = idDGA;
            }
            else
            {
                terceroDGA = dbSIM.PERSONAL_DGA.Find(id);
            }

            return LoadTerceroDGADatos(terceroDGA);
        }

        public ActionResult LoadTerceroDGADatos(PERSONAL_DGA terceroDGA)
        {
            if (terceroDGA.TERCERO != null && terceroDGA.TERCERO.ID_ACTIVIDADECONOMICA != null)
            {
                ACTIVIDAD_ECONOMICA actividadEconomica = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICA == terceroDGA.TERCERO.ID_ACTIVIDADECONOMICA).First();
                ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
            }

            return PartialView("_frmAdministrarTerceroDGA", terceroDGA);
        }

        public ActionResult gvwAdministrarProfesionalesDGA(int id, bool readOnly)
        {
            var modelProfesionales = from personalDGA in dbSIM.PERSONAL_DGA
                                     join tercero in dbSIM.TERCERO on personalDGA.ID_TERCERO equals tercero.ID_TERCERO
                                     join natural in dbSIM.NATURAL on tercero.ID_TERCERO equals natural.ID_TERCERO
                                     join profesion in dbSIM.PROFESION on natural.ID_PROFESION equals profesion.ID_PROFESION into JoinedProfesion
                                     from profesion in JoinedProfesion.DefaultIfEmpty()
                                     where personalDGA.ID_DGA == id
                                     select new
                                     {
                                         personalDGA.ID_DGA,
                                         personalDGA.ID_PERSONALDGA,
                                         personalDGA.ID_TERCERO,
                                         RAZON_SOCIAL = tercero.S_RSOCIAL,
                                         N_DOCUMENTO = tercero.N_DOCUMENTON,
                                         personalDGA.S_TIPOPERSONAL,
                                         S_ESRESPONSABLE = personalDGA.S_ESRESPONSABLE == "S" ? "SI" : "NO",
                                         personalDGA.N_DEDICACION,
                                         personalDGA.N_EXPERIENCIA,
                                         personalDGA.S_OBSERVACION,
                                         CORREO_ELECTRONICO = tercero.S_CORREO,
                                         TELEFONO = tercero.N_TELEFONO,
                                         PROFESION = profesion.S_NOMBRE
                                     };

            ViewBag.ID = id;
            ViewBag.ReadOnly = readOnly;

            return PartialView("_gvwAdministrarProfesionalesDGA", modelProfesionales);
        }

        /// <summary>
        /// Actualiza un tercero Natural o Jurídico con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del tercero</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize(Roles = "ATERCERO")]
        public ActionResult TerceroUpdate(TERCERO item)
        {
            string[] nombres;
            string[] apellidos;
            var model = dbSIM.TERCERO;
            int idRepresentanteLegal = -1;
            TERCERO modelItem;

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca si el tercero existe
                    modelItem = model.FirstOrDefault(it => it.ID_TERCERO == item.ID_TERCERO);
                    if (modelItem != null)
                    {
                        var modelTercero = from tercero in dbSIM.TERCERO
                                           where tercero.ID_TIPODOCUMENTO == item.ID_TIPODOCUMENTO && tercero.N_DOCUMENTON == item.N_DOCUMENTON && tercero.ID_TERCERO != item.ID_TERCERO
                                           select tercero;

                        if (modelTercero.ToList().Count == 0)
                        {
                            this.UpdateModel(modelItem);

                            // Tercero Jurídico
                            if (item.ID_TIPODOCUMENTO == 2)
                            {
                                long identificacionRL;

                                if (long.TryParse(Request.Params["TERCERO.N_DOCUMENTON"], out identificacionRL))
                                {
                                    int tipoDocumentoRL = int.Parse(Request.Params["TERCERO.ID_TIPODOCUMENTO_VI"]);

                                    // Busca si el representante legal existe como tercero natural
                                    TERCERO terceroRepLegal = dbSIM.TERCERO.Where(t => t.ID_TIPODOCUMENTO == tipoDocumentoRL && t.N_DOCUMENTON == identificacionRL).FirstOrDefault();
                                    if (terceroRepLegal == null) // Si no existe lo crea con los datos suministrados por el usuario
                                    {
                                        nombres = Request.Params["txtNombres"].Split(' ');
                                        apellidos = Request.Params["txtApellidos"].Split(' ');

                                        var modelRL = new TERCERO();
                                        modelRL.ID_TIPODOCUMENTO = (int?)tipoDocumentoRL;
                                        modelRL.N_DOCUMENTON = (long?)identificacionRL;
                                        modelRL.N_DOCUMENTO = identificacionRL;
                                        modelRL.N_DIGITOVER = (short?)ObtenerDigitoVerificacion(Request.Params["TERCERO.N_DOCUMENTON"]);
                                        modelRL.NATURAL = new NATURAL();
                                        modelRL.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                                        modelRL.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                                        modelRL.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                                        modelRL.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                                        modelRL.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];
                                        modelRL.S_CORREO = Request.Params["TERCERO.S_CORREO"];
                                        modelRL.S_WEB = Request.Params["TERCERO.S_WEB"];

                                        dbSIM.Entry(modelRL).State = EntityState.Added;
                                        dbSIM.SaveChanges();

                                        idRepresentanteLegal = modelRL.ID_TERCERO;

                                        modelItem.JURIDICA.ID_TERCEROREPLEGAL = idRepresentanteLegal;
                                    }
                                }
                                else
                                {
                                    modelItem.JURIDICA.ID_TERCEROREPLEGAL = null;
                                }
                            }
                            else // Tercero Natural
                            {
                                nombres = Request.Params["txtNombres"].Split(' ');
                                apellidos = Request.Params["txtApellidos"].Split(' ');

                                modelItem.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                                modelItem.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                                modelItem.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                                modelItem.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                                modelItem.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];
                            }

                            dbSIM.SaveChanges();
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Los datos suministrados corresponden a un tercero existente. Por favor verifique el tipo de identificación y número de documento ingresado.";
                        }
                    }
                    else
                    {
                        // Busca si el documento que se está intentando ingresar ya existe
                        modelItem = model.FirstOrDefault(it => it.ID_TIPODOCUMENTO == item.ID_TIPODOCUMENTO && it.N_DOCUMENTON == item.N_DOCUMENTON);

                        if (modelItem == null)
                        {
                            item.N_DOCUMENTO = (long)item.N_DOCUMENTON;

                            // Tercero Jurídico
                            if (item.ID_TIPODOCUMENTO == 2)
                            {
                                long identificacionRL;

                                if (long.TryParse(Request.Params["TERCERO.N_DOCUMENTON"], out identificacionRL))
                                {
                                    int tipoDocumentoRL = int.Parse(Request.Params["TERCERO.ID_TIPODOCUMENTO_VI"]);

                                    // Busca si el representante legal existe como tercero natural
                                    TERCERO terceroRepLegal = dbSIM.TERCERO.Where(t => t.ID_TIPODOCUMENTO == tipoDocumentoRL && t.N_DOCUMENTON == identificacionRL).FirstOrDefault();
                                    if (terceroRepLegal == null) // Si no existe lo crea con los datos suministrados por el usuario
                                    {
                                        nombres = Request.Params["txtNombres"].Split(' ');
                                        apellidos = Request.Params["txtApellidos"].Split(' ');

                                        var modelRL = new TERCERO();
                                        modelRL.ID_TIPODOCUMENTO = (int?)tipoDocumentoRL;
                                        modelRL.N_DOCUMENTON = (long?)identificacionRL;
                                        modelRL.N_DOCUMENTO = identificacionRL;
                                        modelRL.N_DIGITOVER = (short?)ObtenerDigitoVerificacion(Request.Params["TERCERO.N_DOCUMENTON"]);
                                        modelRL.NATURAL = new NATURAL();
                                        modelRL.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                                        modelRL.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                                        modelRL.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                                        modelRL.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                                        modelRL.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];
                                        modelRL.S_CORREO = Request.Params["TERCERO.S_CORREO"];
                                        modelRL.S_WEB = Request.Params["TERCERO.S_WEB"];

                                        dbSIM.Entry(modelRL).State = EntityState.Added;
                                        dbSIM.SaveChanges();

                                        idRepresentanteLegal = modelRL.ID_TERCERO;
                                        item.JURIDICA.ID_TERCEROREPLEGAL = idRepresentanteLegal;
                                    }
                                }
                                //else
                                //{
                                //    modelItem.JURIDICA.ID_TERCEROREPLEGAL = null;
                                //}
                            }
                            else // Tercero Natural
                            {
                                nombres = Request.Params["txtNombres"].Split(' ');
                                apellidos = Request.Params["txtApellidos"].Split(' ');

                                item.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                                item.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                                item.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                                item.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                                item.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];
                            }

                            model.Add(item);
                            dbSIM.SaveChanges();

                            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) == null)
                            {
                                SIM.Areas.Seguridad.Models.PROPIETARIO terceroUsuario = new PROPIETARIO();
                                var modelPropietario = dbSIM.PROPIETARIO;

                                terceroUsuario.ID_TERCERO = item.ID_TERCERO;
                                terceroUsuario.ID_USUARIO = int.Parse(User.Identity.GetUserId());
                                terceroUsuario.D_INICIO = DateTime.Today;
                                dbSIM.Entry(terceroUsuario).State = EntityState.Added;
                                dbSIM.SaveChanges();

                                //return RedirectToAction("LoadInstalacion", "Instalacion", new { idTercero = item.ID_TERCERO });
                            }
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "El tercero ya existe. Por favor verifique el tipo de identificación y número de documento ingresado.";
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["ErrorMessage"] = e.Message;
                }
            }
            else
                ViewData["ErrorMessage"] = "Please, correct all errors.";

            /*TERCERO tercero = dbGeneral.TERCERO.Find(item.ID_TERCERO);

            if (tercero.ID_ACTIVIDADECONOMICA != null)
            {
                ACTIVIDAD_ECONOMICA actividadEconomica = dbGeneral.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICA == tercero.ID_ACTIVIDADECONOMICA).First();
                ViewBag.ActividadEconomica = actividadEconomica.S_NOMBRE;
            }
            ViewBag.tipoTercero = (tercero.ID_TIPODOCUMENTO == 2 ? "J" : "N");

            PROPIETARIO propietario = dbSeguridad.PROPIETARIO.Where(p => p.ID_TERCERO == item.ID_TERCERO).FirstOrDefault();
            if (propietario != null)
                ViewBag.Propietario = propietario.ID_USUARIO;

            return View("Tercero", item);*/

            if (ViewData["ErrorMessage"] == null)
            {
                // Después de actualizar el tercero se carga con los datos actualizados
                return RedirectToAction("LoadTercero", new { id = item.ID_TERCERO });
            }
            else
            {
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
                }

                return View("Tercero", item);
            }
        }

        /// <summary>
        /// Actualiza un tercero Natural asociado a un DGA con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del tercero</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult TerceroDGAUpdate(PERSONAL_DGA item)
        {
            string[] nombres;
            string[] apellidos;
            var modelTercero = dbSIM.TERCERO;
            var model = dbSIM.PERSONAL_DGA;

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca si el tercero existe
                    var modelItem = model.FirstOrDefault(it => it.ID_PERSONALDGA == item.ID_PERSONALDGA);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        
                        nombres = Request.Params["txtNombres"].Split(' ');
                        apellidos = Request.Params["txtApellidos"].Split(' ');

                        modelItem.TERCERO.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                        modelItem.TERCERO.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                        modelItem.TERCERO.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                        modelItem.TERCERO.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                        modelItem.TERCERO.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];

                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        item.TERCERO.N_DOCUMENTO = (long)item.TERCERO.N_DOCUMENTON;

                        nombres = Request.Params["txtNombres"].Split(' ');
                        apellidos = Request.Params["txtApellidos"].Split(' ');

                        item.TERCERO.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
                        item.TERCERO.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
                        item.TERCERO.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
                        item.TERCERO.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
                        item.TERCERO.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];

                        item.S_ESRESPONSABLE = (item.S_ESRESPONSABLE == null ? "N" : item.S_ESRESPONSABLE);

                        model.Add(item);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return null;
        }

        /// <summary>
        /// Inserta tercero Natural con los datos suministrados
        /// </summary>
        /// <param name="item">Estructura del modelo que almacena los datos del tercero</param>
        /// <returns></returns>
        [Authorize(Roles = "CTERCERO")]
        public string TerceroAddNew(TERCERO item)
        {
            string[] nombres = Request.Params["txtNombres"].Split(' ');
            string[] apellidos = Request.Params["txtApellidos"].Split(' ');

            int tipoDocumento = int.Parse(Request.Params["TERCERO.ID_TIPODOCUMENTO_VI"]);
            long identificacion = long.Parse(Request.Params["TERCERO.N_DOCUMENTON"]);

            var model = new TERCERO();
            model.ID_TIPODOCUMENTO = (int?)tipoDocumento;
            model.N_DOCUMENTON = (long?)identificacion;
            model.N_DOCUMENTO = identificacion;
            model.N_DIGITOVER = (short?)ObtenerDigitoVerificacion(Request.Params["TERCERO.N_DOCUMENTON"]);
            model.NATURAL = new NATURAL();
            model.NATURAL.S_NOMBRE1 = (nombres.Length > 0 ? nombres[0] : null);
            model.NATURAL.S_NOMBRE2 = (nombres.Length > 1 ? nombres[1] : null);
            model.NATURAL.S_APELLIDO1 = (apellidos.Length > 0 ? apellidos[0] : null);
            model.NATURAL.S_APELLIDO2 = (apellidos.Length > 1 ? apellidos[1] : null);
            model.S_RSOCIAL = Request.Params["txtNombres"] + " " + Request.Params["txtApellidos"];
            model.S_CORREO = Request.Params["TERCERO.S_CORREO"];
            model.S_WEB = Request.Params["TERCERO.S_WEB"];

            dbSIM.Entry(model).State = EntityState.Added;
            dbSIM.SaveChanges();

            return "OK";

            /*
            var model = dbGeneral.TERCERO;

            item.N_DOCUMENTO = (long)item.N_DOCUMENTON;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    dbGeneral.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Por favor, revise los errores.";

            if (item.ID_TIPODOCUMENTO == 2)
            {
                ViewBag.tipoTercero = "J";
            }
            else
            {
                ViewBag.tipoTercero = "N";
            }

            return View("Tercero", item);
            */
        }

        /// <summary>
        /// Elimina el tercero suministrado
        /// </summary>
        /// <param name="ID_TERCERO">Id del tercero que se va a borrar</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ETERCERO")]
        public ActionResult TerceroDelete(System.Int32 ID_TERCERO)
        {
            var model = dbSIM.TERCERO;

            try
            {
                var item = model.FirstOrDefault(it => it.ID_TERCERO == ID_TERCERO);
                if (item != null)
                {
                    if (item.ID_TIPODOCUMENTO == 2)
                    {
                        var modelJuridica = dbSIM.JURIDICA;
                        var itemJuridica = modelJuridica.FirstOrDefault(it => it.ID_TERCERO == ID_TERCERO);

                        if (item != null)
                        {
                            modelJuridica.Remove(itemJuridica);
                        }
                    }
                    else
                    {
                        var modelNatural = dbSIM.NATURAL;
                        var itemNatural = modelNatural.FirstOrDefault(it => it.ID_TERCERO == ID_TERCERO);

                        if (item != null)
                        {
                            modelNatural.Remove(itemNatural);
                        }
                    }

                    model.Remove(item);
                }

                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return gvwAdministrarTercero();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InstalacionDelete(System.Int32 ID_INSTALACION, int terceroID)
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

            return gvwAdministrarInstalacionesTercero(terceroID);
        }

        /*
        [ValidateInput(false)]
        public ActionResult UsuariosGridViewPartial()
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
                if (Request.Params["gvwUsuarios$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwUsuarios$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    lcbolFiltro = true;
                    break;
                }
            }

            if (lcbolFiltro)
                return PartialView("_UsuariosGridViewPartial", model.ToList());
            else
                return PartialView("_UsuariosGridViewPartial", model.Where(p => p.ID_USUARIO == -10000).ToList());
        }
        */

        /// <summary>
        /// Carga y valida los datos del formulario de tercero de acuerdo a los parámetros suministrados. Si el tercero no existe devuelve un mensaje de error.
        /// </summary>
        /// <param name="id">Id del tercero que se va a consultar</param>
        /// <param name="tipodoc">Id del tipo de documento del tercero</param>
        /// <param name="documento">Numero del documento del tercero</param>
        /// <param name="ok">Mensaje que devuelve en el caso que el tercero con el documento suministrado exista</param>
        /// <param name="alerta">Mensaje que devuelve en el caso que el tercero con el documento suministrado NO exista</param>
        /// <param name="permiteGuardar">Variable que determina cuando debe habilitar en la vista devuelta la posibilidad de almacenar el tercero</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult frmAdministrarTerceroBasico(int? id, int? tipodoc, string documento, string ok, string alerta, bool? permiteGuardar)
        {
            NATURAL model;

            ViewBag.Mensaje = "";
            ViewBag.TipoMensaje = "OK"; // OK - Verde, Alerta - Rojo
            ViewBag.Existe = false;
            ViewBag.PermiteGuardar = false;

            permiteGuardar = (permiteGuardar == null ? false : permiteGuardar);

            if (id != null)
            {
                model = dbSIM.NATURAL.Where(tercero => tercero.ID_TERCERO == id).FirstOrDefault();
                ViewBag.Existe = true;
            }
            else
            {
                if (tipodoc != null)
                {
                    long identificacion = long.Parse(documento);

                    TERCERO terceroRepLegal = dbSIM.TERCERO.Where(tercero => tercero.ID_TIPODOCUMENTO == tipodoc && tercero.N_DOCUMENTON == identificacion).FirstOrDefault();
                    if (terceroRepLegal != null)
                    {
                        model = dbSIM.NATURAL.Where(tercero => tercero.ID_TERCERO == terceroRepLegal.ID_TERCERO).FirstOrDefault();
                        if (model == null)
                        {
                            model = new NATURAL();
                            model.ID_TERCERO = -1;
                        }
                        else
                        {
                            ViewBag.Mensaje = ok; // "El Representante Legal se encuentra registrado en el Sistema.";
                            ViewBag.Existe = true;
                        }
                    }
                    else
                    {
                        model = new NATURAL();
                        model.TERCERO = new TERCERO();
                        model.ID_TERCERO = -1;
                        model.TERCERO.ID_TIPODOCUMENTO = tipodoc;
                        model.TERCERO.N_DOCUMENTON = identificacion;
                        model.TERCERO.N_DIGITOVER = (short?)ObtenerDigitoVerificacion(identificacion.ToString());

                        ViewBag.Mensaje = alerta; // "El Representante Legal NO existe,al guardar se creara uno con los nuevos datos.";
                        ViewBag.TipoMensaje = "Alerta";
                        ViewBag.PermiteGuardar = permiteGuardar;
                    }
                }
                else
                {
                    model = new NATURAL();
                    model.ID_TERCERO = -1;
                }
            }
            return PartialView("_frmAdministrarTerceroBasico", model);
        }

        /// <summary>
        /// Carga los datos del tercero asociado a la identificación suministrada
        /// </summary>
        /// <param name="tipodoc">Id del tipo de documento del tercero</param>
        /// <param name="documento">Numero del documento del tercero</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string CargarDatosTercero(int tipodoc, string documento)
        {
            NATURAL model;

            long identificacion = long.Parse(documento);

            TERCERO tercero = dbSIM.TERCERO.Where(t => t.ID_TIPODOCUMENTO == tipodoc && t.N_DOCUMENTON == identificacion).FirstOrDefault();
            if (tercero != null)
            {
                model = dbSIM.NATURAL.Where(t => t.ID_TERCERO == tercero.ID_TERCERO).FirstOrDefault();
                if (model != null)
                {
                    var datosTercero = new
                    {
                        ID = model.ID_TERCERO,
                        Nombres = model.S_NOMBRE1.Trim() + (model.S_NOMBRE2 == null || model.S_NOMBRE2.Trim() == "" ? "" : " " + model.S_NOMBRE2),
                        Apellidos = model.S_APELLIDO1.Trim() + (model.S_APELLIDO2 == null || model.S_APELLIDO2.Trim() == "" ? "" : " " + model.S_APELLIDO2),
                        Email = tercero.S_CORREO,
                        WebSite = tercero.S_WEB
                    };

                    return JsonConvert.SerializeObject(datosTercero);
                    //return model.S_NOMBRE1.Trim() + (model.S_NOMBRE2.Trim() == "" ? "" : " " + model.S_NOMBRE2) + '|' + model.S_APELLIDO1.Trim() + (model.S_APELLIDO2.Trim() == "" ? "" : " " + model.S_APELLIDO2) + "|" + tercero.S_CORREO + "|" + tercero.S_WEB;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Genera el digito de verificación de un número de documento
        /// </summary>
        /// <param name="numeroDocumento">Número de documento</param>
        /// <returns></returns>
        private short ObtenerDigitoVerificacion(string numeroDocumento)
        {
            StringBuilder ceros = new StringBuilder();
            int longitud = numeroDocumento.Length;
            int[] liPeso = { 71, 67, 59, 53, 47, 43, 41, 37, 29, 23, 19, 17, 13, 7, 3 };
            string numeroDocumentoCompleto;
            int suma = 0;
            int digitoVerificacion;

            for (int i = 1; i <= (15 - longitud); i++)
            {
                ceros.Append("0");
            }

            numeroDocumentoCompleto = ceros.ToString() + numeroDocumento;

            for (int i = 0; i < 15; i++)
            {
                suma += int.Parse(numeroDocumentoCompleto.Substring(i, 1)) * liPeso[i];
            }

            digitoVerificacion = suma % 11;

            if (digitoVerificacion >= 2)
            {
                digitoVerificacion = 11 - digitoVerificacion;
            }

            return (short)digitoVerificacion;
        }

        [ValidateInput(false)]
        [Authorize(Roles = "VTERCERO")]
        public ActionResult gvwAdministrarUsuariosTercero(int? terceroID)
        {
            var model = from propietario in dbSIM.PROPIETARIO
                        join usuarios in dbSIM.USUARIO on propietario.ID_USUARIO equals usuarios.ID_USUARIO
                        where propietario.ID_TERCERO == (terceroID == null ? -10000 : terceroID)
                        select new
                        {
                            propietario.ID_PROPIETARIO,
                            propietario.ID_TERCERO,
                            propietario.ID_USUARIO,
                            usuarios.S_LOGIN,
                            usuarios.S_NOMBRES,
                            usuarios.S_APELLIDOS
                        };


            return PartialView("_gvwAdministrarUsuariosTercero", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CTERCERO")]
        public ActionResult gvwAdministrarUsuariosTerceroAddNew(SIM.Areas.Seguridad.Models.PROPIETARIO item, int? terceroID, int? usuarioID)
        {
            var model = dbSIM.PROPIETARIO;
            if (ModelState.IsValid)
            {
                try
                {
                    item.ID_TERCERO = terceroID;
                    item.ID_USUARIO = usuarioID;
                    item.D_INICIO = DateTime.Today;
                    model.Add(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var modelPropietarios = from propietario in dbSIM.PROPIETARIO
                                    join usuarios in dbSIM.USUARIO on propietario.ID_USUARIO equals usuarios.ID_USUARIO
                                    where propietario.ID_TERCERO == (terceroID == null ? -10000 : terceroID)
                                    select new
                                    {
                                        propietario.ID_PROPIETARIO,
                                        propietario.ID_TERCERO,
                                        propietario.ID_USUARIO,
                                        usuarios.S_LOGIN,
                                        usuarios.S_NOMBRES,
                                        usuarios.S_APELLIDOS
                                    };

            return PartialView("_gvwAdministrarUsuariosTercero", modelPropietarios.ToList());
        }
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ATERCERO")]
        public ActionResult gvwAdministrarUsuariosTerceroUpdate(SIM.Areas.Seguridad.Models.PROPIETARIO item)
        {
            var model = dbSIM.PROPIETARIO;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_PROPIETARIO == item.ID_PROPIETARIO);
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
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwAdministrarUsuariosTercero", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "DTERCERO")]
        public ActionResult gvwAdministrarUsuariosTerceroDelete(System.Int32 ID_PROPIETARIO)
        {
            var model = dbSIM.PROPIETARIO;

            try
            {
                var item = model.FirstOrDefault(it => it.ID_PROPIETARIO == ID_PROPIETARIO);
                if (item != null)
                    model.Remove(item);
                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("_gvwAdministrarUsuariosTercero", model.ToList());
        }

        [ValidateInput(false)]
        [Authorize(Roles = "VTERCERO")]
        public ActionResult gvwAdministrarContactosTercero(int terceroID)
        {
            var modelRP = (from contactos in dbSIM.CONTACTOS
                           where contactos.ID_JURIDICO == terceroID
                           select contactos).Max(c => c.D_INICIO);

            var model = from contactos in dbSIM.CONTACTOS
                        join tercero in dbSIM.TERCERO on contactos.ID_TERCERO_NATURAL equals tercero.ID_TERCERO
                        join terceroNatural in dbSIM.NATURAL on tercero.ID_TERCERO equals terceroNatural.ID_TERCERO
                        where contactos.ID_JURIDICO == terceroID && (!modelRP.HasValue || (contactos.D_INICIO == modelRP.Value && contactos.TIPO == "R"))
                        orderby contactos.D_INICIO descending
                        select contactos;

            ViewBag.terceroID = terceroID;

            return PartialView("_gvwAdministrarContactosTercero", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "CTERCERO")]
        public ActionResult gvwAdministrarContactosTerceroAddNew(CONTACTOS item, int terceroID)
        {
            var model = dbSIM.CONTACTOS;
            if (ModelState.IsValid)
            {
                try
                {
                    TERCERO modelTercero = (from tercero in dbSIM.TERCERO
                                            where tercero.ID_TIPODOCUMENTO == item.TERCERO.ID_TIPODOCUMENTO && tercero.N_DOCUMENTON == item.TERCERO.N_DOCUMENTON
                                            select tercero).FirstOrDefault();

                    var modelItem = new CONTACTOS { ID_JURIDICO = terceroID, ID_TERCERO_NATURAL = modelTercero.ID_TERCERO, TIPO = item.TIPO, D_INICIO = DateTime.Now };
                    dbSIM.Entry(modelItem).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var modelRP = (from contactos in dbSIM.CONTACTOS
                           where contactos.ID_JURIDICO == terceroID
                           select contactos).Max(c => c.D_INICIO);

            var modelContactos = from contactos in dbSIM.CONTACTOS
                        join tercero in dbSIM.TERCERO on contactos.ID_TERCERO_NATURAL equals tercero.ID_TERCERO
                        join terceroNatural in dbSIM.NATURAL on tercero.ID_TERCERO equals terceroNatural.ID_TERCERO
                        where contactos.ID_JURIDICO == terceroID && (!modelRP.HasValue || (contactos.D_INICIO == modelRP.Value && contactos.TIPO == "R"))
                        orderby contactos.D_INICIO descending
                        select contactos;

            return PartialView("_gvwAdministrarContactosTercero", modelContactos.ToList());
        }
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "ATERCERO")]
        public ActionResult gvwAdministrarContactosTerceroUpdate(CONTACTOS item, int terceroID)
        {
            var model = dbSIM.CONTACTOS;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_CONTACTO == item.ID_CONTACTO);
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
                ViewData["EditError"] = "Please, correct all errors.";

            var modelRP = (from contactos in dbSIM.CONTACTOS
                           where contactos.ID_JURIDICO == terceroID
                           select contactos).Max(c => c.D_INICIO);

            var modelContactos = from contactos in dbSIM.CONTACTOS
                                 join tercero in dbSIM.TERCERO on contactos.ID_TERCERO_NATURAL equals tercero.ID_TERCERO
                                 join terceroNatural in dbSIM.NATURAL on tercero.ID_TERCERO equals terceroNatural.ID_TERCERO
                                 where contactos.ID_JURIDICO == terceroID && (!modelRP.HasValue || (contactos.D_INICIO == modelRP.Value && contactos.TIPO == "R"))
                                 orderby contactos.D_INICIO descending
                                 select contactos;

            return PartialView("_gvwAdministrarContactosTercero", modelContactos.ToList());
        }
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "DTERCERO")]
        public ActionResult gvwAdministrarContactosTerceroDelete(System.Int32 ID_CONTACTO, int terceroID)
        {
            var model = dbSIM.CONTACTOS;

            try
            {
                var item = model.FirstOrDefault(it => it.ID_CONTACTO == ID_CONTACTO);
                if (item != null)
                    model.Remove(item);
                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            var modelRP = (from contactos in dbSIM.CONTACTOS
                           where contactos.ID_JURIDICO == terceroID
                           select contactos).Max(c => c.D_INICIO);

            var modelContactos = from contactos in dbSIM.CONTACTOS
                                 join tercero in dbSIM.TERCERO on contactos.ID_TERCERO_NATURAL equals tercero.ID_TERCERO
                                 join terceroNatural in dbSIM.NATURAL on tercero.ID_TERCERO equals terceroNatural.ID_TERCERO
                                 where contactos.ID_JURIDICO == terceroID && (!modelRP.HasValue || (contactos.D_INICIO == modelRP.Value && contactos.TIPO == "R"))
                                 orderby contactos.D_INICIO descending
                                 select contactos;

            return PartialView("_gvwAdministrarContactosTercero", modelContactos.ToList());
        }

        [ValidateInput(false)]
        public ActionResult ValidarTercero(int idTercero, int tipoDoc, long documento)
        {
            var model = from tercero in dbSIM.TERCERO
                        where tercero.ID_TIPODOCUMENTO == tipoDoc && tercero.N_DOCUMENTON == documento && tercero.ID_TERCERO != idTercero
                        select tercero;

            if (model.ToList().Count == 0)
                return null;
            else
            {
                ViewData["ErrorMessage"] = "El tercero ya existe. Por favor verifique el tipo de identificación y número de documento ingresado.";
                return PartialView("_frmErrorText");
            }
        }

        public ActionResult LinksTercero()
        {
            int idTercero;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) == null)
            {
                return PartialView("_LinksTercero", null);
            }
            else
            {
                idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                TERCERO tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == idTercero).FirstOrDefault();

                if (tercero != null)
                {
                    if (tercero.ACTIVIDAD_ECONOMICA == null || tercero.ACTIVIDAD_ECONOMICA.S_VERSION != "4")
                    {
                        return PartialView("_LinksTercero", tercero);
                    }
                }

                return null;
            }
        }
    }
}