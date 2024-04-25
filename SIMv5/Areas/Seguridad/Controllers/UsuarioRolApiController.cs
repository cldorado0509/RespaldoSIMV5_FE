using Microsoft.AspNet.Identity;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Data.General;
using SIM.Data.Seguridad;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador Account: Gestiona el inicio de sesión de usuario en la aplicación
    /// </summary>
    public class UsuarioRolApiController : ApiController
    {
        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        public struct DatosRechazo
        {
            public int idRolSolicitado;
            public string comentarios;
        }

        //Se crea objeto IdentityManager para administrar los metodos de busqueda, creacion y administracion de un usuario y su sesion
        private IdentityManager _idManager = new IdentityManager();
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
        private EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

        private System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Valida la información de registro del usuario, si es correcta lo registra en la aplicación e inicia sesión. LLamda POST a la acción.
        /// </summary>
        /// <param name="model">modelo con la información del usuario para registrarse en la aplicación</param>
        /// <returns>Vista home del SIM o vista para registrar usuario en de caso de error </returns>
        [HttpPost, ActionName("AsignarRoles")]
        public datosRespuesta AsignarRoles(ROLESUSUARIO[] rolesUsuario)
        {
            int idUsuario = Convert.ToInt32(User.Identity.GetUserId());

            try
            {
                foreach (ROLESUSUARIO rol in rolesUsuario)
                {
                    var rolUsuario = dbSeguridad.USUARIO_ROL.Where(r => r.ID_USUARIO == idUsuario && r.ID_ROL == rol.ID_ROL).FirstOrDefault();
                    if (rol.SEL)
                    {
                        if (rolUsuario == null)
                        {
                            rolUsuario = new USUARIO_ROL() { ID_ROL = rol.ID_ROL, ID_USUARIO = idUsuario };
                            dbSeguridad.Entry(rolUsuario).State = EntityState.Added;
                            dbSeguridad.SaveChanges();
                        }
                    }
                    else
                    {
                        if (rolUsuario != null)
                        {
                            dbSeguridad.Entry(rolUsuario).State = EntityState.Deleted;
                            dbSeguridad.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Error Asignando Roles" };
            }

            return new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "Roles Asignados Satisfactoriamente" };
        }

        /// <summary>
        /// Valida la información de registro del usuario, si es correcta lo registra en la aplicación e inicia sesión. LLamda POST a la acción.
        /// </summary>
        /// <param name="model">modelo con la información del usuario para registrarse en la aplicación</param>
        /// <returns>Vista home del SIM o vista para registrar usuario en de caso de error </returns>
        [HttpPost, ActionName("AsignarRolesUsuario")]
        public datosRespuesta AsignarRolesUsuario(int idUsuario, ROLESUSUARIO[] rolesUsuario)
        {
            string rolesAsignados = "";

            /*int? idUsuarioActual = null;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuarioActual = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }*/

            int idUsuarioActual = Convert.ToInt32(User.Identity.GetUserId());

            try
            {
                foreach (ROLESUSUARIO rol in rolesUsuario)
                {
                    var rolUsuario = dbSeguridad.USUARIO_ROL.Where(r => r.ID_USUARIO == idUsuario && r.ID_ROL == rol.ID_ROL).FirstOrDefault();
                    if (rol.SEL)
                    {
                        if (rolUsuario == null)
                        {
                            rolUsuario = new USUARIO_ROL() { ID_ROL = rol.ID_ROL, ID_USUARIO = idUsuario };
                            dbSeguridad.Entry(rolUsuario).State = EntityState.Added;
                            dbSeguridad.SaveChanges();
                        }

                        if (rolesAsignados == "")
                            rolesAsignados = rol.ID_ROL.ToString();
                        else
                            rolesAsignados += "," + rol.ID_ROL.ToString();
                    }
                    else
                    {
                        if (rolUsuario != null)
                        {
                            dbSeguridad.Entry(rolUsuario).State = EntityState.Deleted;
                            dbSeguridad.SaveChanges();
                        }
                    }
                }

                var rolSolicitadoUsuario = (from rolSolicitado in dbSeguridad.ROL_SOLICITADO
                                            where rolSolicitado.ID_USUARIO == idUsuario && rolSolicitado.S_ESTADO == "V"
                                            select rolSolicitado).FirstOrDefault();

                if (rolSolicitadoUsuario != null)
                {
                    rolSolicitadoUsuario.S_ESTADO = "A";
                    rolSolicitadoUsuario.ID_USUARIO_ADM = idUsuarioActual;
                    rolSolicitadoUsuario.S_ROLES_ASIG = rolesAsignados;
                    rolSolicitadoUsuario.D_FECHA_ASIG_RECHAZO = DateTime.Now;

                    dbSeguridad.Entry(rolSolicitadoUsuario).State = EntityState.Modified;
                    dbSeguridad.SaveChanges();
                }

                var usuarioEmail = (from usuarioRolAsignado in dbSeguridad.USUARIO
                                    where usuarioRolAsignado.ID_USUARIO == idUsuario
                                    select usuarioRolAsignado.S_EMAIL).FirstOrDefault();

                if (usuarioEmail != null && usuarioEmail.Trim() != "")
                {
                    var emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoRolesUsuarioAsignados.html")));
                    emailHtml.Replace("[usuario]", usuarioEmail);

                    Utilidades.Email.EnviarEmail(usuarioEmail, "Registro Satisfactorio en el SIM - Roles Asignados", emailHtml.ToString());
                }
            }
            catch (Exception e)
            {
                return new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Error Asignando Roles" };
            }

            return new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "Roles Asignados Satisfactoriamente" };
        }

        /// <summary>
        /// Consulta los roles solicitados por un usuario y que aun no hayan sido procesados
        /// </summary>
        /// <param name="id">ID del Usuario</param>
        /// <returns>Lista de Roles que el Usuario solicitó</returns>
        [HttpGet, ActionName("RolesSolicitados")]
        public List<int> RolesSolicitados(int id)
        {
            List<int> rolesSolicitados = new List<int>();

            var solicitud = (from solicitudRoles in dbSeguridad.ROL_SOLICITADO
                             where solicitudRoles.ID_USUARIO == id && solicitudRoles.S_ESTADO == "V"
                             select solicitudRoles.S_ROLES_SOL).FirstOrDefault();

            if (solicitud != null)
            {
                foreach (string rol in solicitud.Split(','))
                {
                    rolesSolicitados.Add(Convert.ToInt32(rol));
                }
            }

            return rolesSolicitados;
        }

        [HttpGet, ActionName("RolesUsuarioExterno")]
        //public List<ROLESUSUARIO> RolesUsuarioExterno(int id)
        public List<int> RolesUsuarioExterno(int id)
        {
            /*var model = from roles in dbSeguridad.ROL.Where(r => r.S_TIPO == "E")
                        join usuario in dbSeguridad.USUARIO_ROL.Where(u => u.ID_USUARIO == id) on roles.ID_ROL equals usuario.ID_ROL into rolesUsuario
                        from usuario in rolesUsuario.DefaultIfEmpty()
                        orderby roles.S_NOMBRE
                        select new ROLESUSUARIO()
                        {
                            SEL = usuario == null ? false : true,
                            ID_ROL = roles.ID_ROL,
                            S_NOMBRE = roles.S_NOMBRE
                        };*/

            var model = from roles in dbSeguridad.ROL.Where(r => r.S_TIPO == "E")
                        join rolesUsuario in dbSeguridad.USUARIO_ROL.Where(u => u.ID_USUARIO == id) on roles.ID_ROL equals rolesUsuario.ID_ROL
                        select roles.ID_ROL;

            return model.ToList();
        }

        /// <summary>
        /// Activa un usuario que realizó el registro.
        /// </summary>
        /// <param name="idRolSolicitado">Id de la solicitud del registro</param>
        /// <returns></returns>
        [HttpGet, ActionName("ActivarUsuario")]
        public datosRespuesta GetActivarUsuario(int idRolSolicitado)
        {
            int idRol;
            int idUsuarioActual = Convert.ToInt32(User.Identity.GetUserId());

            var rolSolicitado = (from rs in dbSeguridad.ROL_SOLICITADO
                                 where rs.ID_ROL_SOLICITADO == idRolSolicitado
                                 select rs).FirstOrDefault();

            if (rolSolicitado == null)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "No existe Solicitud de Registro relacionada" };
            }
            else
            {
                var idUsuario = rolSolicitado.ID_USUARIO;

                //using (var trans = new TransactionScope()) // No funciona aun
                //{
                try
                {
                    idRol = Convert.ToInt32(Utilidades.Data.ObtenerValorParametro("RolAdministradorEmpresa"));

                    var rolUsuarioAdmin = dbSeguridad.USUARIO_ROL.Where(r => r.ID_USUARIO == idUsuario && r.ID_ROL == idRol).FirstOrDefault();

                    if (rolUsuarioAdmin == null)
                    {
                        rolUsuarioAdmin = new USUARIO_ROL() { ID_ROL = idRol, ID_USUARIO = idUsuario };
                        dbSeguridad.Entry(rolUsuarioAdmin).State = EntityState.Added;
                        dbSeguridad.SaveChanges();
                    }

                    if (rolSolicitado.S_ROLES_SOL != null && rolSolicitado.S_ROLES_SOL.Trim() != "")
                    {
                        foreach (string rol in rolSolicitado.S_ROLES_SOL.Split(','))
                        {
                            idRol = Convert.ToInt32(rol);

                            var rolUsuario = dbSeguridad.USUARIO_ROL.Where(r => r.ID_USUARIO == idUsuario && r.ID_ROL == idRol).FirstOrDefault();

                            if (rolUsuario == null)
                            {
                                rolUsuario = new USUARIO_ROL() { ID_ROL = idRol, ID_USUARIO = idUsuario };
                                dbSeguridad.Entry(rolUsuario).State = EntityState.Added;
                                dbSeguridad.SaveChanges();
                            }
                        }
                    }

                    rolSolicitado.S_ESTADO = "A";
                    rolSolicitado.ID_USUARIO_ADM = idUsuarioActual;
                    rolSolicitado.S_ROLES_ASIG = rolSolicitado.S_ROLES_SOL;
                    rolSolicitado.D_FECHA_ASIG_RECHAZO = DateTime.Now;

                    dbSeguridad.Entry(rolSolicitado).State = EntityState.Modified;
                    dbSeguridad.SaveChanges();

                    var usuario = (from u in dbSeguridad.USUARIO
                                   where u.ID_USUARIO == rolSolicitado.ID_USUARIO
                                   select u).FirstOrDefault();

                    if (usuario != null)
                    {
                        usuario.S_ESTADO = "A";

                        dbSeguridad.Entry(usuario).State = EntityState.Modified;
                        dbSeguridad.SaveChanges();
                    }

                    var usuarioEmail = (from usuarioRolAsignado in dbSeguridad.USUARIO
                                        where usuarioRolAsignado.ID_USUARIO == idUsuario
                                        select usuarioRolAsignado.S_EMAIL).FirstOrDefault();

                    if (usuarioEmail != null && usuarioEmail.Trim() != "")
                    {
                        var emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoRolesUsuarioAsignados.html")));
                        emailHtml.Replace("[usuario]", usuarioEmail);

                        Utilidades.Email.EnviarEmail(usuarioEmail, "Registro Satisfactorio en el SIM - Administrador de Tercero", emailHtml.ToString());
                    }

                    // Se finaliza el trámite
                    ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                    dbTramites.SP_AVANZA_TAREA(0, rolSolicitado.CODTRAMITE, 0, 0, 0, "0", "", rtaResultado);

                    var dv = Utilidades.Data.ObtenerDigitoVerificacion(((long)rolSolicitado.N_DOCUMENTO).ToString()).ToString();
                    var documento = Convert.ToInt64(rolSolicitado.N_DOCUMENTO.ToString() + dv);

                    var tercero = (from t in dbSeguridad.TERCERO
                                   where t.N_DOCUMENTON == rolSolicitado.N_DOCUMENTO || t.N_DOCUMENTO == documento
                                   select t).FirstOrDefault();

                    if (tercero == null) // Tercero no existe, por lo tanto se crea
                    {
                        tercero = new TERCERO();

                        tercero.N_DOCUMENTON = rolSolicitado.N_DOCUMENTO;
                        tercero.N_DIGITOVER = Convert.ToByte(dv);
                        tercero.N_DOCUMENTO = documento;
                        tercero.S_RSOCIAL = rolSolicitado.S_RSOCIAL;
                        tercero.ID_TIPODOCUMENTO = 2;
                        tercero.ID_ESTADO = 1;
                        tercero.D_REGISTRO = DateTime.Now;
                        tercero.JURIDICA = new JURIDICA();
                        tercero.JURIDICA.S_RSOCIAL = rolSolicitado.S_RSOCIAL;

                        dbSeguridad.Entry(tercero).State = EntityState.Added;
                        dbSeguridad.SaveChanges();
                    }

                    var propietario = (from p in dbSeguridad.PROPIETARIO
                                       where p.ID_USUARIO == idUsuario && p.ID_TERCERO == tercero.ID_TERCERO
                                       select p).FirstOrDefault();

                    if (propietario == null) // No existe la relación entre el usuario y el tercero
                    {
                        propietario = new PROPIETARIO();

                        propietario.ID_TERCERO = tercero.ID_TERCERO;
                        propietario.ID_USUARIO = idUsuario;
                        propietario.D_INICIO = DateTime.Now;

                        dbSeguridad.Entry(propietario).State = EntityState.Added;
                        dbSeguridad.SaveChanges();
                    }

                    //trans.Complete();
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Usuario Rol [GetActivarUsuario - " + idRolSolicitado.ToString() + " ] : Se presentó un error Activando el Usuario.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                    return new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Error Activando Usuario. " + error.Message };
                }
                //}
            }

            return new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "Usuario Activado Satisfactoriamente" };
        }

        /// <summary>
        /// Activa un usuario que realizó el registro.
        /// </summary>
        /// <param name="idRolSolicitado">Id de la solicitud del registro</param>
        // <returns></returns>
        [HttpPost, ActionName("RechazarUsuario")]
        public datosRespuesta PostRechazarUsuario(DatosRechazo datosRechazo)
        {
            int idUsuarioActual = Convert.ToInt32(User.Identity.GetUserId());

            var rolSolicitado = (from rs in dbSeguridad.ROL_SOLICITADO
                                 where rs.ID_ROL_SOLICITADO == datosRechazo.idRolSolicitado
                                 select rs).FirstOrDefault();

            if (rolSolicitado == null)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "No existe Solicitud de Registro relacionada" };
            }
            else
            {
                var idUsuario = rolSolicitado.ID_USUARIO;

                rolSolicitado.S_ESTADO = "I";
                rolSolicitado.ID_USUARIO_ADM = idUsuarioActual;
                rolSolicitado.D_FECHA_ASIG_RECHAZO = DateTime.Now;
                rolSolicitado.S_COMENTARIOS = datosRechazo.comentarios;

                dbSeguridad.Entry(rolSolicitado).State = EntityState.Modified;
                dbSeguridad.SaveChanges();

                // Se finaliza el trámite
                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                dbTramites.SP_AVANZA_TAREA(0, rolSolicitado.CODTRAMITE, 0, 0, 0, "0", datosRechazo.comentarios, rtaResultado);

                var usuario = (from u in dbSeguridad.USUARIO
                               where u.ID_USUARIO == idUsuario
                               select u).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.S_ESTADO = "I";

                    dbSeguridad.Entry(usuario).State = EntityState.Modified;
                    dbSeguridad.SaveChanges();
                }

                var usuarioEmail = (from usuarioRolAsignado in dbSeguridad.USUARIO
                                    where usuarioRolAsignado.ID_USUARIO == idUsuario
                                    select usuarioRolAsignado.S_EMAIL).FirstOrDefault();

                if (usuarioEmail != null && usuarioEmail.Trim() != "")
                {
                    var emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoRegistroRechazado.html")));
                    emailHtml.Replace("[usuario]", usuarioEmail);

                    Utilidades.Email.EnviarEmail(usuarioEmail, "Registro Rechazado en el SIM - Administrador de Tercero", emailHtml.ToString());
                }
            }

            return new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "Registro Rechazado Satisfactoriamente" };
        }
    }
}