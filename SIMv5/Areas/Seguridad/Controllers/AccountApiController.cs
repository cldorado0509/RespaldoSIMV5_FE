using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using AspNet.Identity.Oracle;
using Microsoft.Owin.Security;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Tramites.Models;
using SIM.Data;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.Net.Mail;
using SIM.Utilidades;
using System.Transactions;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Data.Seguridad;
using SIM.Models;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador Account: Gestiona el inicio de sesión de usuario en la aplicación
    /// </summary>
    public class AccountApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public int codigoRespuesta;
            public string tipoRespuesta; // OK, Error, Ya Tiene Tercero, 
            public string detalleRespuesta;
        }

        //Se crea objeto IdentityManager para administrar los metodos de busqueda, creacion y administracion de un usuario y su sesion
        private IdentityManager _idManager = new IdentityManager();
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
        private EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();
        private EntitiesControlOracle dbControl = new EntitiesControlOracle();

        private System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Valida la información de registro del usuario, si es correcta lo registra en la aplicación e inicia sesión. LLamda POST a la acción.
        /// </summary>
        /// <param name="model">modelo con la información del usuario para registrarse en la aplicación</param>
        /// <returns>Vista home del SIM o vista para registrar usuario en de caso de error </returns>
        [HttpPost, ActionName("Register")]
        public datosRespuesta Register(RegisterViewModel model)
        {
            string emailFrom;
            string emailSMTPServer;
            string emailSMTPUser;
            string emailSMTPPwd;
            ApplicationUser user;
            StringBuilder emailHtml;
            int? rolAdministradorEmpresa = null;
            string errorMsg = "";

            emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
            emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

            model.UserName = model.Email;

            try
            {
                rolAdministradorEmpresa = Convert.ToInt32(Utilidades.Data.ObtenerValorParametro("RolAdministradorEmpresa"));
            }
            catch (Exception error)
            {
                errorMsg = error.Message;
                rolAdministradorEmpresa = null;

                return new datosRespuesta { codigoRespuesta = -150, tipoRespuesta = "Error", detalleRespuesta = "Error de Procesamiento. Por favor intentar más tarde." + (errorMsg == "" ? "" : "\r\n" + errorMsg) };
            }

            if (rolAdministradorEmpresa == null) // Error de Configuración
            {
                return new datosRespuesta { codigoRespuesta = -100, tipoRespuesta = "Error", detalleRespuesta = "Error de Configuración. Por favor contactar al administrador del Sistema." + (errorMsg == "" ? "" : "\r\n" + errorMsg)};
            }

            // Si es empresa se verifica si ya hay un usuario con perfil administrador, de lo contrario se retorna para que el usuario adjunte archivos y posteriormente se cree el trámite para la creación del usuario
            if (model.TipoPersonaUsuario == "J" && model.Type == 0)
            {
                // Verifica primero si el usuario con ese email ya existe y está relacionado a un tercero
                var usuario = (from usuarioEmail in dbSeguridad.USUARIO
                               where usuarioEmail.S_LOGIN.Trim() == model.Email.Trim() && (usuarioEmail.S_ESTADO == "A" || usuarioEmail.S_ESTADO == "V")
                               select usuarioEmail).FirstOrDefault();

                if (usuario != null) // email ya registrado 
                {
                    return new datosRespuesta { codigoRespuesta = -1, tipoRespuesta = "Error", detalleRespuesta = "NO se puede realizar el registro. El Usuario ya se encuentra registrado." };

                    /*
                    var propietario = (from propietarioUsuario in dbSeguridad.PROPIETARIO
                                       where propietarioUsuario.ID_USUARIO == usuario.ID_USUARIO
                                       select propietarioUsuario).FirstOrDefault();

                    if (propietario != null) // el usuario está relacionado con un tercero
                    {
                        var tercero = (from terceroUsuario in dbSeguridad.TERCERO
                                       where terceroUsuario.ID_TERCERO == propietario.ID_TERCERO
                                       select terceroUsuario).FirstOrDefault();

                        if (tercero == null)
                        {
                            // Inconsistencia, si está relacionado con un tercero, éste tiene que existir
                            return new datosRespuesta { codigoRespuesta = -2, tipoRespuesta = "Error", detalleRespuesta = "Datos Inconsistentes. Por favor contactar al administrador del Sistema." };
                        }
                        else
                        {
                            if (tercero.N_DOCUMENTON.ToString() == model.LastName) // El tercero que tiene relacionado es el mismo que digitaron en el registro
                            {
                                // Si ya se encuentra registrado y relacionado al tercero, se verifica que ya hay un usuario Administrador de Empresa
                                var propietarioAdministrador = (from propietarioUsuario in dbSeguridad.PROPIETARIO
                                                                join rolUsuario in dbSeguridad.USUARIO_ROL on propietarioUsuario.ID_USUARIO equals rolUsuario.ID_USUARIO
                                                                where propietarioUsuario.ID_TERCERO == tercero.ID_TERCERO && rolUsuario.ID_ROL == rolAdministradorEmpresa
                                                                select propietarioUsuario).FirstOrDefault();

                                if (propietarioAdministrador == null) // No hay administrador para la empresa
                                {
                                    // Cómo no hay administrador, debe preguntar si desea registrarse como administrador, de lo contrario no se puede registrar hasta que haya un administrador para la empresa

                                    return new datosRespuesta { codigoRespuesta = 1, tipoRespuesta = "OK", detalleRespuesta = "Usuario Existe y está relacionado al tercero al cual se registra. Desea registrarse como administrador ?" };
                                }
                                else
                                {
                                    // Cómo hay administrador se le pregunta al usuario a que rol aspira
                                    return new datosRespuesta { codigoRespuesta = 2, tipoRespuesta = "OK", detalleRespuesta = "Usuario Existe y está relacionado al tercero al cual se registra. Seleccionar los roles que desea habilitar." };
                                }
                            }
                            else // El tercero que tiene relacionado no es el mismo que digitaron
                            {
                                return new datosRespuesta { codigoRespuesta = -3, tipoRespuesta = "Error", detalleRespuesta = "El Usuario que intenta registrar ya tiene tercero asociado (diferente al digitado en el registro)" };
                            }
                        }
                    }
                    else // El usuario no está relacionado a un tercero
                    {
                        // Verificar si el tercero Existe y si tiene usuario Administrador

                        var propietarioAdministrador = (from propietarioUsuario in dbSeguridad.PROPIETARIO
                                                        join rolUsuario in dbSeguridad.USUARIO_ROL on propietarioUsuario.ID_USUARIO equals rolUsuario.ID_USUARIO
                                                        join tercero in dbSeguridad.TERCERO on propietarioUsuario.ID_TERCERO equals tercero.ID_TERCERO
                                                        where tercero.N_DOCUMENTON.ToString() == model.LastName  && rolUsuario.ID_ROL == rolAdministradorEmpresa
                                                        select propietarioUsuario).FirstOrDefault();

                        if (propietarioAdministrador == null) // No Existe el tercero o no tiene usuario con rol Adiminstrador
                        {
                            return new datosRespuesta { codigoRespuesta = 3, tipoRespuesta = "OK", detalleRespuesta = "Desea registrarse como administrador del Tercero relacionado ?" };
                        }
                        else
                        {
                            return new datosRespuesta { codigoRespuesta = 4, tipoRespuesta = "OK", detalleRespuesta = "Seleccionar los roles que desea habilitar." };
                        }
                    }*/
                }
                else // email no registrado
                {
                    // Verificar si el tercero Existe y si tiene usuario Administrador
                    var propietarioAdministrador = (from propietarioUsuario in dbSeguridad.PROPIETARIO
                                                    join rolUsuario in dbSeguridad.USUARIO_ROL on propietarioUsuario.ID_USUARIO equals rolUsuario.ID_USUARIO
                                                    join tercero in dbSeguridad.TERCERO on propietarioUsuario.ID_TERCERO equals tercero.ID_TERCERO
                                                    where tercero.N_DOCUMENTON.ToString() == model.Nit && rolUsuario.ID_ROL == rolAdministradorEmpresa
                                                    select propietarioUsuario).FirstOrDefault();

                    if (propietarioAdministrador == null) // No Existe el tercero o no tiene usuario con rol Adiminstrador
                    {
                        string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsTempPath"];

                        string[] archivos = Directory.GetFiles(pathDocumentos, "R_" + model.Nit + "_*");
                        foreach (string archivo in archivos)
                        {
                            File.Delete(archivo);
                        }

                        return new datosRespuesta { codigoRespuesta = 5, tipoRespuesta = "OK", detalleRespuesta = "El Usuario será creado. Desea registrarse como administrador del Tercero relacionado ?" };
                    }
                    else
                    {
                        return new datosRespuesta { codigoRespuesta = 6, tipoRespuesta = "OK", detalleRespuesta = "El Usuario será creado. Seleccionar los roles que desea habilitar." };
                    }
                }
            }

            //using (var trans = new TransactionScope())
            {
                // Verifica primero si el usuario con ese email ya existe y está relacionado a un tercero
                var usuario = (from usuarioEmail in dbSeguridad.USUARIO
                               where usuarioEmail.S_LOGIN.Trim() == model.Email.Trim()
                               select usuarioEmail).FirstOrDefault();

                if (usuario != null) // email ya registrado 
                {
                    try
                    {
                        user = _idManager.RegisterUserBD(model.GetUser(), model.Password, true);
                    }
                    catch (Exception error)
                    {
                        return new datosRespuesta { codigoRespuesta = -200, tipoRespuesta = "Error", detalleRespuesta = "Error Registrando Usuario (1a)" };
                    }
                }
                else
                {
                    //Se registra el usuario en base de datos
                    try
                    {
                        user = _idManager.RegisterUserBD(model.GetUser(), model.Password);
                    }
                    catch (Exception error)
                    {
                        return new datosRespuesta { codigoRespuesta = -200, tipoRespuesta = "Error", detalleRespuesta = "Error Registrando Usuario (2a)" };
                    }
                }

                if (user.isAuthenticated)
                {
                    // En caso de ser Administrador de Empresa se genera el trámite y se comienza en el flujo correspondiente.
                    if (model.Type == 1)
                    {
                        var terceroRegistro = (from tercero in dbSeguridad.TERCERO
                                                where tercero.N_DOCUMENTON.ToString() == model.Nit
                                                select tercero).FirstOrDefault();

                        var codProceso = Convert.ToDecimal(Utilidades.Data.ObtenerValorParametro("IdProcesoRegistro"));
                        var codFuncionario = Convert.ToDecimal(Utilidades.Data.ObtenerValorParametro("CodFuncionarioRegistro"));
                        ObjectParameter respCodTramite = new ObjectParameter("respCodTramite", typeof(decimal));
                        ObjectParameter respCodTarea = new ObjectParameter("respCodTarea", typeof(decimal));
                        ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                        dbTramites.SP_NUEVO_TRAMITE_REGISTRO(codProceso, codFuncionario, "Registro de Usuario " + model.UserName + " para ser habilitado como Administrador del Tercero '" + model.Nit + "-" + model.RazonSocial + "'", respCodTramite, respCodTarea, rtaResultado);

                        if (rtaResultado.Value.ToString() != "OK")
                            return new datosRespuesta { codigoRespuesta = -210, tipoRespuesta = "Error", detalleRespuesta = "Error Registrando Trámite" };

                        var solicitudRoles = new ROL_SOLICITADO();
                        solicitudRoles.ID_USUARIO = Convert.ToInt32(user.Id);
                        solicitudRoles.S_ROLES_SOL = model.Roles;
                        solicitudRoles.D_FECHA_SOL = DateTime.Now;
                        solicitudRoles.S_ESTADO = "V"; // V Validación, A Aceptado y Procesado, R Rechazado
                        if (terceroRegistro != null)
                            solicitudRoles.ID_TERCERO = terceroRegistro.ID_TERCERO;
                        solicitudRoles.S_ADMINISTRADOR = "S";
                        solicitudRoles.N_DOCUMENTO = Convert.ToInt64(model.Nit);
                        solicitudRoles.S_RSOCIAL = model.RazonSocial;
                        solicitudRoles.CODTRAMITE = Convert.ToInt32(respCodTramite.Value);

                        dbSeguridad.Entry(solicitudRoles).State = EntityState.Added;
                        dbSeguridad.SaveChanges();

                        AsignarDocumentosTemporalesTramite(model.Nit, codProceso, Convert.ToDecimal(respCodTramite.Value), Convert.ToDecimal(respCodTarea.Value), codFuncionario);

                        try
                        {
                            emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoRegistroValidacion.html")));
                            emailHtml.Replace("[usuario]", model.UserName);
                            emailHtml.Replace("[nombre]", model.FirstName + " " + model.LastName);
                        }
                        catch (Exception error)
                        {
                            return new datosRespuesta { codigoRespuesta = -201, tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Solicitud de Roles" };
                        }

                        try
                        {
                            string listaCorreos  = "";

                            try
                            {
                                var correosCopia = dbSeguridad.Database.SqlQuery<string>("SELECT S_EMAIL_NOTIFICACION FROM SEGURIDAD.ROL_NOTIFICACION WHERE ID_ROL IN (" + model.Roles + ")").ToList<string>();
                                listaCorreos = String.Join<string>(";", correosCopia);
                            }
                            catch { }

                            Utilidades.Email.EnviarEmail(emailFrom, model.Email, "", (listaCorreos == null || listaCorreos.Trim() == "" ? "" : listaCorreos), "Registro Satisfactorio en el SIM - Pendiente Validación para Activación del Usuario", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd, null);
                        }
                        catch (Exception error)
                        {
                            Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                            return new datosRespuesta { codigoRespuesta = -202, tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Solicitud de Roles" };
                        }
                    }
                    else // En caso de solicitar rol, se envia el correo al Administrador de la Empresa y se registra la solicitud
                    {
                        var propietarioAdministrador = (from propietarioUsuario in dbSeguridad.PROPIETARIO
                                                        join rolUsuario in dbSeguridad.USUARIO_ROL on propietarioUsuario.ID_USUARIO equals rolUsuario.ID_USUARIO
                                                        join tercero in dbSeguridad.TERCERO on propietarioUsuario.ID_TERCERO equals tercero.ID_TERCERO
                                                        where tercero.N_DOCUMENTON.ToString() == model.Nit && rolUsuario.ID_ROL == rolAdministradorEmpresa
                                                        select propietarioUsuario).FirstOrDefault();

                        var solicitudRoles = new ROL_SOLICITADO();
                        solicitudRoles.ID_USUARIO = Convert.ToInt32(user.Id);
                        solicitudRoles.S_ROLES_SOL = model.Roles;
                        solicitudRoles.D_FECHA_SOL = DateTime.Now;
                        solicitudRoles.S_ESTADO = "V"; // V Validación, A Aceptado y Procesado, R Rechazado
                        solicitudRoles.ID_TERCERO = (int)propietarioAdministrador.ID_TERCERO;

                        dbSeguridad.Entry(solicitudRoles).State = EntityState.Added;
                        dbSeguridad.SaveChanges();

                        try
                        {
                            emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoSolicitudRolesAdministrador.html")));
                            emailHtml.Replace("[usuario]", model.UserName);
                            emailHtml.Replace("[roles]", model.RolesNombres);
                        }
                        catch (Exception error)
                        {
                            return new datosRespuesta { codigoRespuesta = -201, tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Solicitud de Roles" };
                        }

                        try
                        {
                            Utilidades.Email.EnviarEmail(emailFrom, propietarioAdministrador.USUARIO.S_EMAIL, "Solicitud de habilitación de permisos en el SIM", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
                        }
                        catch (Exception error)
                        {
                            Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                            return new datosRespuesta { codigoRespuesta = -202, tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Solicitud de Roles" };
                        }

                        try
                        {
                            emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoSolicitudRolesUsuario.html")));
                            emailHtml.Replace("[usuario]", propietarioAdministrador.USUARIO.S_EMAIL);
                            emailHtml.Replace("[nombre]", model.FirstName + " " + model.LastName);
                        }
                        catch (Exception error)
                        {
                            return new datosRespuesta { codigoRespuesta = -201, tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Solicitud de Roles" };
                        }

                        try
                        {
                            Utilidades.Email.EnviarEmail(emailFrom, model.Email, "Registro de Usuario en el SIM - Pendiente activación", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
                        }
                        catch (Exception error)
                        {
                            Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                            return new datosRespuesta { codigoRespuesta = -202, tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Solicitud de Roles" };
                        }
                    }

                    /* // Versión previa donde se enviar el enlace de activación inmediatamente
                    string enlaceValidador = Url.Link("SeguridadDefault", new { Controller = "Account", Action = "ValidarUsuario", id = model.GetUser().Validador });
                    enlaceValidador = "<a href='" + enlaceValidador + "'>" + enlaceValidador + "</a>";

                    try
                    {
                        emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoRegistro.html")));
                        emailHtml.Replace("[usuario]", model.UserName);
                        emailHtml.Replace("[validador]", enlaceValidador);
                    }
                    catch (Exception error)
                    {
                        return new datosRespuesta { codigoRespuesta = -201, tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Validación" };
                    }

                    try
                    {
                        Utilidades.Email.EnviarEmail(emailFrom, model.Email, "Validación de usuario registrados en el SIM", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
                    }
                    catch (Exception error)
                    {
                        Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                        return new datosRespuesta { codigoRespuesta = -202, tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Validación" };
                    }*/

                    //****trans.Complete();
                    return new datosRespuesta { codigoRespuesta = 0, tipoRespuesta = "OK", detalleRespuesta = "Usuario Registrado Satisfactoriamente" };
                }
                else
                {
                    return new datosRespuesta { codigoRespuesta = -203, tipoRespuesta = "Error", detalleRespuesta = user.errAuthenticate };
                }
            }
        }

        private void AsignarDocumentosTemporalesTramite(string nit, decimal codProceso, decimal codTramite, decimal codTarea, decimal codFuncionario)
        {
            Data.Tramites.TBRUTAPROCESO rutaProceso = dbSeguridad.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == codProceso).FirstOrDefault();
            string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsTempPath"];
            string pathDocumentosTemporalesTramite = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(codTramite), 100);
            string filename;
            string descripcion;
            string ruta;
            int orden = 1;
            ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

            string[] archivos = Directory.GetFiles(pathDocumentos, "R_" + nit + "_*");
            foreach (string archivo in archivos)
            {
                descripcion = Path.GetFileName(archivo).Substring(Path.GetFileName(archivo).IndexOf('_', 2) + 1);
                ruta = pathDocumentosTemporalesTramite + codTramite.ToString("0") + "_" + descripcion;

                if (!Directory.Exists(pathDocumentosTemporalesTramite))
                    Directory.CreateDirectory(pathDocumentosTemporalesTramite);

                System.IO.File.Move(archivo, ruta);

                dbTramites.SP_ASIGNAR_TEMPORAL_TRAMITE(codTramite, codTarea, codFuncionario, orden++, -1, Path.GetFileNameWithoutExtension(descripcion), ruta, rtaResultado);
            }
        }

        [HttpPost, ActionName("RecoverPassword")]
        public datosRespuesta RecoverPassword(RegisterViewModel model)
        {
            string emailFrom;
            string emailSMTPServer;
            string emailSMTPUser;
            string emailSMTPPwd;
            string validacionHash;
            ApplicationUser user;
            StringBuilder emailHtml;

            if (model.Hash == null || model.Hash.Trim() == "")
            {
                if (model.Email == null)
                {
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Datos Inconsistentes del Usuario. Contacte al Administrador del Sistema." };
                }

                emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
                emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

                var usuario = dbSeguridad.USUARIO.Where(u => u.S_EMAIL == model.Email).FirstOrDefault();

                if (usuario == null)
                {
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "El correo electrónico NO está matriculado en el sistema." };
                }
                else if (usuario.S_ESTADO != "A")
                {
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "El usuario NO se encuentra activo en el Sistema y por lo tanto no puede recuperar la contraseña. Por favor contactar al Administrador del Sistema." };
                }

                model.UserName = usuario.S_LOGIN;
                model.FirstName = usuario.S_NOMBRES;
                model.LastName = usuario.S_APELLIDOS;

                var validador = model.GetUser().Recuperacion;

                usuario.S_VALIDADOR = validador;
                dbSeguridad.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                dbSeguridad.SaveChanges();

                string enlaceValidador = Url.Link("SeguridadDefault", new { Controller = "Account", Action = "RecuperarClave", id = validador });
                enlaceValidador = "<a href='" + enlaceValidador + "'>" + enlaceValidador + "</a>";

                try
                {
                    emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoRegistro.html")));
                    emailHtml.Replace("[usuario]", model.UserName);
                    emailHtml.Replace("[validador]", enlaceValidador);
                }
                catch (Exception error)
                {
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Recuperación de Contraseña" };
                }

                try
                {
                    Utilidades.Email.EnviarEmail(emailFrom, model.Email, "Recuperación de Contraseña para usuario registrado en el SIM", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Recuperación de Contraseña" };
                }

                return new datosRespuesta { tipoRespuesta = "OK", detalleRespuesta = "Recuperación de Contraseña Procesada Satisfactoriamente" };
            }
            else
            {
                var usuario = dbSeguridad.USUARIO.Where(u => u.S_VALIDADOR == model.Hash).FirstOrDefault();

                if (usuario == null)
                {
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Datos Inconsistentes. Contactar al Administrador del Sistema." };
                }

                validacionHash = _idManager.ValidateUserCambioPwd(model.Hash);

                if (validacionHash == "OK")
                {
                    usuario.S_PASSWORD = _idManager.EncriptarPassword(model.Password);
                    dbSeguridad.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    dbSeguridad.SaveChanges();

                    return new datosRespuesta { tipoRespuesta = "OK", detalleRespuesta = "Recuperación de Contraseña Procesada Satisfactoriamente" };
                }
                else
                {
                    return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = validacionHash };
                }
            }
        }

        /*[HttpPost, ActionName("CargarArchivo")]
        public async Task<object> PostCargarArchivo()
        {
            HttpPostedFileBase file = Request.Files["file"];

            // Specifies the target location for the uploaded files
            string targetLocation = Server.MapPath("~/Files/");

            // Specifies the maximum size allowed for the uploaded files (700 kb)
            int maxFileSize = 1024 * 700;

            // Checks whether or not the request contains a file and if this file is empty or not
            if (file == null || file.ContentLength <= 0)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = "File is not specified";
                return new EmptyResult();
            }

            // Checks that the file size does not exceed the allowed size
            if (file.ContentLength > maxFileSize)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = "File is too big";
                return new EmptyResult();
            }

            // Checks that the file is an image
            if (!file.ContentType.Contains("image"))
            {
                Response.StatusCode = 400;
                Response.StatusDescription = "Invalid file type";
                return new EmptyResult();
            }

            try
            {
                string path = System.IO.Path.Combine(targetLocation, file.FileName);
                // Here, make sure that the file will be saved to the required directory.
                // Also, ensure that the client has not uploaded files with malicious content.
                // If all checks are passed, save the file.
                file.SaveAs(path);
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = "Invalid file name";
                return new EmptyResult();
            }
            return new EmptyResult();
        }*/

        [HttpPost, ActionName("CargarArchivo")]
        public async Task<object> PostCargarArchivo(string id)
        {
            int numArchivo = 1;
            //int numeroIntentos = 0;
            //string id;
            string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsTempPath"];
            string filename;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new FileMultipartFormDataStreamProvider(pathDocumentos);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                //for (int i = 0; i < provider.FileData.Count && numeroIntentos < 10; i++)
                for (int i = 0; i < provider.FileData.Count; i++)
                {
                    //filename = "R_" + id + "_" + numArchivo.ToString();
                    filename = "R_" + id + "_" + provider.FileData[i].Headers.ContentDisposition.FileName.Replace("\"", string.Empty);

                    if (!Directory.Exists(pathDocumentos))
                    {
                        Directory.CreateDirectory(pathDocumentos);
                    }

                    //while (System.IO.File.Exists(pathDocumentos + "\\" + filename))
                    //{
                    //    filename = "R_" + id + "_" + (++numArchivo).ToString();
                    //}

                    try // Evita que se trate de crear un archivo con el mismo nombre
                    {
                        System.IO.File.Move(pathDocumentos + "\\" + provider.FileData[i].Headers.ContentDisposition.FileName.Replace("\"", string.Empty), pathDocumentos + "\\" + filename);
                    }
                    catch (Exception e)
                    {
                        //numeroIntentos++;
                        //numArchivo = 1;
                        //i--;
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet, ActionName("Reset")]
        public object GetReset(string id)
        {
            string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsTempPath"];

            string[] archivos = Directory.GetFiles(pathDocumentos, "R_" + id + "_*");
            foreach (string archivo in archivos)
            {
                File.Delete(pathDocumentos + "\\" + archivo);
            }

            /*int numArchivo = 1;
            string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsTempPath"];
            string filename;
            bool finArchivos = false;

            for (int i = 0; !finArchivos; i++)
            {
                filename = "R_" + id + "_" + numArchivo.ToString();

                if (System.IO.File.Exists(pathDocumentos + "\\" + filename))
                {
                    System.IO.File.Delete(pathDocumentos + "\\" + filename);
                }
                else
                {
                    finArchivos = true;
                }

                numArchivo++;
                filename = "R_" + id + "_" + numArchivo.ToString();
            }*/

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET api/<controller>
        [HttpGet]
        [ActionName("UsuariosActivacion")]
        public datosConsulta GetUsuariosActivacion(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                var idFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);

                switch (tipoData)
                {
                    case "r": // reduced
                    case "f": // full
                        {
                            var model = (from rs in dbSeguridad.ROL_SOLICITADO
                                         join u in dbSeguridad.USUARIO on rs.ID_USUARIO equals u.ID_USUARIO
                                         join tt in dbSeguridad.TBTRAMITETAREA on (int?)rs.CODTRAMITE equals (int?)tt.CODTRAMITE
                                         where rs.S_ADMINISTRADOR == "S" && rs.S_ESTADO == "V" && tt.FECHAFIN == null && tt.CODFUNCIONARIO == idFuncionario 
                                         select new
                                         {
                                             rs.ID_ROL_SOLICITADO,
                                             u.ID_USUARIO,
                                             u.S_LOGIN,
                                             rs.N_DOCUMENTO,
                                             rs.S_RSOCIAL,
                                             rs.CODTRAMITE
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from rs in dbSeguridad.ROL_SOLICITADO
                                         join u in dbSeguridad.USUARIO on rs.ID_USUARIO equals u.ID_USUARIO
                                         where rs.S_ADMINISTRADOR == "S" && rs.S_ESTADO == "V"
                                         select new
                                         {
                                             rs.ID_ROL_SOLICITADO,
                                             u.ID_USUARIO,
                                             u.S_LOGIN,
                                             rs.N_DOCUMENTO,
                                             rs.S_RSOCIAL,
                                             rs.CODTRAMITE
                                         });

                            modelData = model;
                        }
                        break;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                if (take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [HttpGet]
        [ActionName("DocumentosTemporales")]
        public datosConsulta GetDocumentosTemporales(int tramite)
        {
            var lcobjDatos = dbSeguridad.Database.SqlQuery<DOCUMENTO_TEMPORAL>("SELECT ID_DOCUMENTO, S_DESCRIPCION "
                + "FROM TRAMITES.DOCUMENTO_TEMPORAL "
                + "WHERE CODTRAMITE = " + tramite.ToString() + " ORDER BY S_DESCRIPCION");

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = lcobjDatos.Count<DOCUMENTO_TEMPORAL>();
            resultado.datos = lcobjDatos.ToList<DOCUMENTO_TEMPORAL>();

            return resultado;
        }
    }

    public class FileMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public FileMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }
}