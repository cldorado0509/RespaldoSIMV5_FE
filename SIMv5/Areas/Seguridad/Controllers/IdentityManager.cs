using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using SIM.Areas.Seguridad.Models;
using System.Configuration;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using SIM.Data;
using System.Data.Common;
using System.Globalization;
using SIM.Data.Seguridad;
using System.Web.Hosting;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Clase IdentityManager: Permite la autenticacion de usuario contra el directorio activo y base de datos, 
    /// además del registro de un usurio independiente del proveedor de autenticacion
    /// </summary>
    public class IdentityManager
    {
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        public DbConnection Conexion
        {
            get { return dbSeguridad.Database.Connection; }
        }

        /// <summary>
        /// Determina si el usuario gestiona su contreña a traves del SIM
        /// </summary>
        /// <param name="idUser">Id del usuario</param>
        /// <returns>Verdadero o falso si el usuario gestiona su contraseña en el SIM</returns>
        public bool UserHasPassword(int idUser)
        {
            USUARIO usuario = dbSeguridad.USUARIO.Find(idUser);
            if (usuario != null)
            {
                var qyul = dbSeguridad.USUARIO_LOGIN.Where(td => td.ID_USUARIO == idUser);
                bool l = qyul.Count() == 0;
                bool p = usuario.S_PASSWORD != null;
                return l && p;
            }
            return false;
        }

        /// <summary>
        /// Busca si el usuario con su login externo ya se encuentra registrado en la base de datos del sistema
        /// </summary>
        /// <param name="userlogin">Informacion del login externo</param>
        /// <returns>ApplicationUser que representa usuario con su información y claims registrada</returns>
        public ApplicationUser FindUser(UserLoginInfo userlogin)
        {
            ApplicationUser usr = new ApplicationUser("");
            // Se busca el usuario en la base de datos de acuerdo a su ProviderKey
            FindUserBD(ref usr, userlogin);

            // Si existe en la base de datos se cargan sus claims
            if (usr.isAuthenticated)
                CargarClaimsUsuario(ref usr);

            return usr;
        }

        /// <summary>
        /// Busca un usuario en el directorio activo y base de datos del sistema, de acuerdo a la informacion de usuario y contraseña proporcionada en el inicio de sesion de la aplicacion
        /// </summary>
        /// <param name="userName">login del usuario</param>
        /// <param name="password">contraseña del usuario</param>
        /// <returns>ApplicationUser que representa usuario con su información y claims registrada</returns>
        public ApplicationUser FindUser(string userName, string password)
        {
            ApplicationUser usr = new ApplicationUser(userName);

            // Verifica si la aplicacion esta configurada para validar usuarios contra un directorio activo
            string mLDAP = ConfigurationManager.AppSettings["LDAP"].ToUpper();
            // si esta configurado contra un directorio activo, se valida el usuario tanto en este como contra la base de datos del sistema
            if (mLDAP == "SI")
            {
                // busca el usuario en el directorio activo
                FindUserAD(ref usr, password);
                // si no lo encuentra lo busca en la base de datos
                if (!usr.isAuthenticated)
                    FindUserBD(ref usr, password);
                else
                {
                    // si el usuario si es autenticado contra el directorio activo se procede a hacer su registro en la aplicacion
                    FindUserBD(ref usr);
                    if (usr.isAuthenticated)
                        RegisterUserAD(ref usr, password);
                }
            }
            else
                // si no esta configurado contra un directorio activo, se valida el usuario solo contra la base de datos del sistema
                FindUserBD(ref usr, password);

            // Si el usuario está autenticado se cargan sus claims
            if (usr.isAuthenticated)
                CargarClaimsUsuario(ref usr);

            return usr;
        }

        public ApplicationUser FindUser(string usuario, int idUsuario)
        {
            ApplicationUser usr = new ApplicationUser(usuario);

            FindUserBD(ref usr, idUsuario);

            if (usr.isAuthenticated)
                CargarClaimsUsuario(ref usr);

            return usr;
        }

        public ApplicationUser RegisterUserBD(ApplicationUser user, string password)
        {
            return RegisterUserBD(user, password, false);
        }

        /// <summary>
        /// Registra un usuario en la aplicacion
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario a registrar</param>
        /// <param name="password">contraseña del usuario</param>
        /// <returns>ApplicationUser que representa el usuario registrado</returns>
        public ApplicationUser RegisterUserBD(ApplicationUser user, string password, bool activar)
        {
            try
            {
                // se valida que el nombre de usuario no exista
                var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN.ToUpper() == user.UserName.ToUpper() || td.S_LOGIN.ToUpper() == user.Email.ToUpper());
                if (dbUser.Count() == 1 && !activar)
                //if (dbUser.FirstOrDefault() != null)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioYaExiste;
                    user.isAuthenticated = false;
                    return user;
                }

                // se valida que el email no exista
                dbUser = dbSeguridad.USUARIO.Where(td => td.S_EMAIL == user.Email);
                if (dbUser.Count() == 1 && !activar)
                //if (dbUser.FirstOrDefault() != null)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjEmailYaExiste;
                    user.isAuthenticated = false;
                    return user;
                }

                // se crea el usuario en la base de datos
                if (!activar)
                    CrearUsuario(ref user, EncriptarPassword(password));
                else
                    ActivarUsuario(ref user, EncriptarPassword(password));

                user.errAuthenticate = string.Empty;
                user.isAuthenticated = true;
            }
            catch (Exception ex)
            {
                user.errAuthenticate = Properties.ResourcesError.errRegistrandoUsuario + " " + ex.Message;
                user.isAuthenticated = false;
            }

            return user;
        }

        /// <summary>
        /// Valida y marca como activo a un usuario en la aplicacion
        /// </summary>
        /// <param name="hash">string con el código de validación</param>
        /// <returns>true Satisfactorio, false Falla</returns>
        public string ValidateUserBD(string hash)
        {
            try
            {
                var dbUser = dbSeguridad.USUARIO.Where(td => td.S_VALIDADOR.ToUpper() == hash.ToUpper());// && td.S_ESTADO == "V");

                if (dbUser.Count() == 1)
                {
                    var user = dbUser.First();

                    if (user.S_ESTADO == "I")
                        return "Usuario Inactivo.";
                    else if (user.S_ESTADO == "A")
                        return "El Usuario ya se encuentra activo. Puede ingresar al Sistema usando el usuario y la contraseña registradas.";
                    else if (user.S_ESTADO != "V")
                        return "Usuario en estado desconocido.";

                    user.S_ESTADO = "A";

                    dbSeguridad.Entry(user).State = EntityState.Modified;
                    dbSeguridad.SaveChanges();

                    try
                    {
                        long nit = Convert.ToInt64(user.S_APELLIDOS);

                        var tercero = dbSeguridad.TERCERO.Where(t => t.N_DOCUMENTON == nit).FirstOrDefault();

                        if (tercero != null)
                        {
                            PROPIETARIO propietario = new PROPIETARIO();
                            propietario.ID_USUARIO = user.ID_USUARIO;
                            propietario.ID_TERCERO = tercero.ID_TERCERO;

                            dbSeguridad.Entry(propietario).State = EntityState.Added;
                            dbSeguridad.SaveChanges();
                        }
                    }
                    catch (Exception error)
                    {
                        // Error debido a que es una persona natural y por lo tanto el apellido no tiene el NIT
                    }

                    return "OK";
                }
                else
                {
                    return "Usuario no Registrado.";
                }
            }
            catch (Exception ex)
            {
                // TODO: Registrar error de validación de usuario
                return Utilidades.Data.ObtenerError(ex);
            }
        }


        /// <summary>
        /// Valida el Usuario que desea asignar contraseña
        /// </summary>
        /// <param name="hash">string con el código de validación</param>
        /// <returns>true Satisfactorio, false Falla</returns>
        public string ValidateUserCambioPwd(string hash)
        {
            try
            {
                var dbUser = dbSeguridad.USUARIO.Where(td => td.S_VALIDADOR.ToUpper() == hash.ToUpper());

                if (dbUser.Count() == 1)
                {
                    var posicionFecha = hash.IndexOf("|");

                    if (posicionFecha < 0)
                        return "Enlace inválido. Por favor contactar al Administrador del Sistema.";
                    else
                    {
                        try
                        {
                            var fecha = DateTime.ParseExact(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(hash.Substring(posicionFecha + 1))), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                            if (Math.Abs(DateTime.Now.Subtract(fecha).TotalMinutes) > 60)
                            {
                                return "Enlace inválido, ha caducado. Por favor realice el proceso nuevamente.";
                            }
                        }
                        catch
                        {
                            return "Enlace inválido, posiblemente fue modificado. Por favor contactar al Administrador del Sistema.";
                        }
                    }
                    return "OK";
                }
                else
                {
                    return "Usuario no registrado o no ha sido solicitada la asignación de contaseña.";
                }
            }
            catch (Exception ex)
            {
                // TODO: Registrar error de validación de usuario
                return Utilidades.Data.ObtenerError(ex);
            }
        }

        /// <summary>
        /// Registra un usuario en la aplicacion, usuario autenticado con proveedor externo
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario a registrar</param>
        /// <param name="userlogin">inforamcion del proveedor externo</param>
        /// <returns>ApplicationUser que representa el usuario registrado</returns>
        public ApplicationUser RegisterUserWeb(ApplicationUser user, UserLoginInfo userlogin)
        {
            try
            {
                // se valida que el nombre de usuario no exista
                var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN == user.UserName);
                if (dbUser.Count() == 1)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioYaExiste;
                    user.isAuthenticated = false;
                    return user;
                }

                // se valida que el email no exista
                dbUser = dbSeguridad.USUARIO.Where(td => td.S_EMAIL == user.Email);
                if (dbUser.Count() == 1)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjEmailYaExiste;
                    user.isAuthenticated = false;
                    return user;
                }

                // se crea el usuario en la base de datos
                CrearUsuario(ref user, string.Empty);
                // se registra el providerKey al usuario
                CrearUsuarioLogin(int.Parse(user.Id), userlogin.LoginProvider, userlogin.ProviderKey);
                user.errAuthenticate = string.Empty;
                user.isAuthenticated = true;
            }
            catch (Exception ex)
            {
                user.errAuthenticate = Properties.ResourcesError.errRegistrandoUsuario + " " + ex.Message;
                user.isAuthenticated = false;
            }

            return user;
        }

        /// <summary>
        /// Autentica un usuario contra el directorio activo
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario a autenticar</param>
        /// <param name="passwordHash">contraseña del usuario</param>
        /// <returns>ApplicationUser que representa el usuario</returns>
        private void FindUserAD(ref ApplicationUser user, string passwordHash)
        {
            string strPath = ConfigurationManager.AppSettings["ServidorLDAP"];
            string strDomain = ConfigurationManager.AppSettings["Dominio"];
            string domainAndUsername = strDomain + @"\" + user.UserName;

            // se busca una entrada en el directorio activo para ese usuario y contraseña
            DirectoryEntry entry = new DirectoryEntry(strPath, domainAndUsername, passwordHash);
            try
            {
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + user.UserName + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioContrasenaIncorrectos;
                    user.isAuthenticated = false;
                }
                else user.isAuthenticated = true;
                obj = null;
                result = null;
            }
            catch (Exception ex)
            {
                user.errAuthenticate = Properties.ResourcesError.errAutenticandoUsuario + " " + ex.Message;
                user.isAuthenticated = false;

                //Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Login [FindUserAD - " + user.UserName + " ] : " + Utilidades.LogErrores.ObtenerError(ex));
            }
            finally
            {
                entry.Dispose();
            }
        }

        /// <summary>
        /// Permite cambiar la contraseña de un usuario de base de datos
        /// </summary>
        /// <param name="idUser">Id del usuario</param>
        /// <param name="currentPassword">contraseña actual del usuario</param>
        /// <param name="newPassword">nueva contraseña del usuario</param>
        /// <returns>Posible error que ocurra durante el cambio de la contraseña</returns>
        public string ChangePasswordUserBD(int idUser, string currentPassword, string newPassword)
        {
            try
            {
                USUARIO usuario = dbSeguridad.USUARIO.Find(idUser);

                // se valida el usuario
                if (usuario == null)
                    return Properties.ResourcesMensajes.msjUsuarioInvalido;
                // se valida contraseña actual
                if (usuario.S_PASSWORD != EncriptarPassword(currentPassword))
                    return Properties.ResourcesMensajes.msjContrasenaIncorrecta;
                // se encripta nueva contraseña y se le asigna al usuario
                usuario.S_PASSWORD = EncriptarPassword(newPassword);
                dbSeguridad.Entry(usuario).State = EntityState.Modified;
                dbSeguridad.SaveChanges();
            }
            catch (Exception ex)
            {
                return Properties.ResourcesError.errInesperado + " " + ex.Message;
            }

            return string.Empty;
        }

        /// <summary>
        /// Busca un usuario en base de datos de acuerdo al providerKey con que se ha logeado
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario</param>
        /// <param name="userlogin">inforamcion del proveedor externo</param>
        private void FindUserBD(ref ApplicationUser user, UserLoginInfo userlogin)
        {
            // se consulta que exista un usuario en la base de datos con el ese ProviderKey
            var qyul = from u in dbSeguridad.USUARIO
                       join ul in dbSeguridad.USUARIO_LOGIN on u.ID_USUARIO equals ul.ID_USUARIO
                       where ul.LOGINPROVIDER == userlogin.LoginProvider && ul.PROVIDERKEY == userlogin.ProviderKey
                       select u;

            if (qyul.Count() > 0)
            {
                // se valida que el usuario este activo
                USUARIO u = qyul.First();
                if (u.D_VENCE < System.DateTime.Now)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioInactivoPorFecha;
                    user.isAuthenticated = false;
                }
                else
                {
                    user.Id = u.ID_USUARIO.ToString();
                    user.UserName = u.S_LOGIN;
                    user.FirstName = u.S_NOMBRES;
                    user.LastName = u.S_APELLIDOS;
                    user.Email = u.S_EMAIL;
                    if (u.ID_GRUPO.HasValue)
                        user.TipoUsuario = u.ID_GRUPO.Value;

                    user.errAuthenticate = string.Empty;
                    user.isAuthenticated = true;
                }
            }
            else
            {
                user.errAuthenticate = string.Empty;
                user.isAuthenticated = false;
            }
        }

        /// <summary>
        /// Busca un usuario en base de datos de acuerdo al usuario y contraseña proporcionada
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario</param>
        /// <param name="passwordHash">contraseña del usuario</param>
        private void FindUserBD(ref ApplicationUser user, string passwordHash)
        {
            // se encripta la contraseña y se busca en base de datos el usuario con ese login contraseña
            string usr = user.UserName;
            string pwd = EncriptarPassword(passwordHash);
            //var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN == usr && td.S_PASSWORD == pwd);
            var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN.ToUpper() == usr.ToUpper() && td.S_ESTADO == "A");

            USUARIO u = dbUser.FirstOrDefault();

            /*var urpmes = (from ur in u.USUARIO_ROL
                          where ur.ID_ROL == 45
                          select ur).FirstOrDefault();

            if ((u != null && u.S_PASSWORD == pwd) || (urpmes != null  && passwordHash == "9631")) *///&&&*** INGRESO SIN CLAVE UNICAMENTE PARA USUARIOS CON PERFIL DE PMES
            // Valida que el usuario exista y la contraseña sea válida
            //if (dbUser.Count() == 1 && usr.ToUpper().Trim() == "ANDRES.ROMERO") // INGRESO SOLAMENTE PARA EL USUARIO DE ANDRES
            //if (dbUser.Count() == 1) //&&&*** INGRESO SIN CONTRASEÑA
            if (u != null && u.S_PASSWORD == pwd)
            {
                // se valida que el usuario este activo
                if (u.D_VENCE < System.DateTime.Now)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioInactivoPorFecha;
                    user.isAuthenticated = false;
                }
                else
                {
                    user.Id = u.ID_USUARIO.ToString();
                    user.errAuthenticate = string.Empty;
                    user.isAuthenticated = true;
                }
            }
            else
            {
                user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioContrasenaIncorrectos;
                user.isAuthenticated = false;
            }
        }

        private void FindUserBD(ref ApplicationUser user, int idUsuario)
        {
            // se encripta la contraseña y se busca en base de datos el usuario con ese login contraseña
            string usr = user.UserName;
            //var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN == usr && td.S_PASSWORD == pwd);
            var dbUser = dbSeguridad.USUARIO.Where(td => td.ID_USUARIO == idUsuario);

            USUARIO u = dbUser.FirstOrDefault();

            // Valida que el usuario exista y la contraseña sea válida
            //if (dbUser.Count() == 1)
            if (u != null)
            {
                user.Id = u.ID_USUARIO.ToString();
                user.FirstName = u.S_NOMBRES;
                user.LastName = u.S_APELLIDOS;
                user.Email = u.S_EMAIL;
                user.errAuthenticate = string.Empty;
                user.isAuthenticated = true;
            }
            else
            {
                user.isAuthenticated = false;
            }
        }

        private void FindUserBD(ref ApplicationUser user)
        {
            // se encripta la contraseña y se busca en base de datos el usuario con ese login contraseña
            string usr = user.UserName;
            var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN.ToUpper() == usr.ToUpper() && td.S_ESTADO == "A");

            USUARIO u = dbUser.FirstOrDefault();

            if (u != null)
            {
                // se valida que el usuario este activo
                if (u.D_VENCE < System.DateTime.Now)
                {
                    user.errAuthenticate = Properties.ResourcesMensajes.msjUsuarioInactivoPorFecha;
                    user.isAuthenticated = false;
                }
                else
                {
                    user.Id = u.ID_USUARIO.ToString();
                    user.errAuthenticate = string.Empty;
                    user.isAuthenticated = true;
                }
            }
        }

        /// <summary>
        /// Registra en base de datos un usuario autenticado contra el directorio activo
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario</param>
        /// <param name="passwordHash">contraseña del usuario</param>
        private void RegisterUserAD(ref ApplicationUser user, string passwordHash)
        {
            // se valida que el usuario no exista en la base de datos, si existe entonces no se hace su registro
            string usr = user.UserName;
            var dbUser = dbSeguridad.USUARIO.Where(td => td.S_LOGIN.ToUpper() == usr.ToUpper());

            if (dbUser.Count() == 1) //usuario existe en base de datos
                user.Id = dbUser.First().ID_USUARIO.ToString();
            else
            {
                try
                {
                    string strPath = ConfigurationManager.AppSettings["ServidorLDAP"];
                    string strDomain = ConfigurationManager.AppSettings["Dominio"];
                    string domainAndUsername = strDomain + @"\" + user.UserName;
                    string nombre = "", apellido = "", strlogin = "", strmail = "", title = "";

                    // se busca el usuario en el directorio activo y se carga su informacion personal para asociarlo al usuario en base de datos
                    DirectoryEntry entry = new DirectoryEntry(strPath, domainAndUsername, passwordHash);
                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(&(objectClass=user)(SAMAccountName=" + user.UserName + "))";
                    foreach (SearchResult sResultSet in search.FindAll())
                    {
                        apellido = GetProperty(sResultSet, "sn");
                        nombre = GetProperty(sResultSet, "givenName");
                        strlogin = GetProperty(sResultSet, "SAMAccountName");
                        strmail = GetProperty(sResultSet, "mail");
                        title = GetProperty(sResultSet, "title");
                    }

                    user.FirstName = nombre.ToUpper();
                    user.LastName = apellido.ToUpper();
                    user.Email = strmail;

                    // se buscar el id de grupo asociado al puesto del usuario segun el directorio activo, esto para cargar los roles que pueda tener asociado su cargo
                    var qg = from g in dbSeguridad.GRUPO
                             where g.S_NOMBRE.ToUpper() == title.ToUpper()
                             select g.ID_GRUPO;
                    if (qg.Count() > 0)
                        user.TipoUsuario = qg.First();

                    // se crea el usuario en la base de datos
                    CrearUsuario(ref user, string.Empty);

                }
                catch (Exception ex)
                {
                    user.errAuthenticate = Properties.ResourcesError.errRegistrandoUsuario + " " + ex.Message;
                    user.isAuthenticated = false;
                    return;
                }
            }

            user.errAuthenticate = string.Empty;
            user.isAuthenticated = true;
        }

        /// <summary>
        /// Crear un usuario en base de datos 
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario</param>
        /// <param name="passwordHash">contraseña del usuario</param>
        private void CrearUsuario(ref ApplicationUser user, string passwordHash)
        {
            // se carga modelo USUARIO con la informacion proporcionada por el usuario de aplicacion
            USUARIO nUser = new USUARIO();

            nUser.S_NOMBRES = user.FirstName.ToUpper();
            nUser.S_APELLIDOS = user.LastName.ToUpper();
            nUser.S_LOGIN = user.UserName;
            nUser.S_EMAIL = user.Email;
            nUser.ID_GRUPO = user.TipoUsuario;
            nUser.D_REGISTRO = DateTime.Now;
            nUser.D_VENCE = DateTime.Now.AddYears(5);
            nUser.S_TIPO = user.TipoPersonaUsuario;
            nUser.S_SUPERUSUARIO = "G"; //**C*
            if (!string.IsNullOrEmpty(passwordHash))
            {
                nUser.S_PASSWORD = passwordHash;
            }
            nUser.S_VALIDADOR = user.Validador;
            nUser.S_ESTADO = "V";

            dbSeguridad.USUARIO.Add(nUser);
            dbSeguridad.SaveChanges();
            //dbSeguridad.SaveChangesAnonymous();

            user.Id = nUser.ID_USUARIO.ToString();

            if (nUser.ID_GRUPO != null)
            {
                // se cargan los roles asociados a su cargo
                var roles = from cr in dbSeguridad.GRUPO_ROL
                            where cr.ID_GRUPO == nUser.ID_GRUPO
                            select cr.ID_ROL;

                foreach (var id in roles)
                {
                    USUARIO_ROL ur = new USUARIO_ROL();
                    ur.ID_USUARIO = nUser.ID_USUARIO;
                    ur.ID_ROL = id;

                    dbSeguridad.USUARIO_ROL.Add(ur);
                }

                if (roles.Count() > 0)
                {
                    dbSeguridad.SaveChanges();
                    //dbSeguridad.SaveChangesAnonymous();
                }
            }
        }

        /// <summary>
        /// Activar un usuario existente en base de datos 
        /// </summary>
        /// <param name="user">ApplicationUser con la informacion del usuario</param>
        /// <param name="passwordHash">contraseña del usuario</param>
        private void ActivarUsuario(ref ApplicationUser user, string passwordHash)
        {
            string login = user.Email.Trim();

            // se carga modelo USUARIO con la informacion proporcionada por el usuario de aplicacion
            USUARIO nUser = (from usuarioEmail in dbSeguridad.USUARIO
                           where usuarioEmail.S_LOGIN.Trim() == login
                           select usuarioEmail).FirstOrDefault();

            nUser.S_NOMBRES = user.FirstName.ToUpper();
            nUser.S_APELLIDOS = user.LastName.ToUpper();
            nUser.S_LOGIN = user.UserName;
            nUser.S_EMAIL = user.Email;
            nUser.ID_GRUPO = user.TipoUsuario;
            nUser.D_REGISTRO = DateTime.Now;
            nUser.D_VENCE = DateTime.Now.AddYears(5);
            nUser.S_TIPO = user.TipoPersonaUsuario;
            nUser.S_SUPERUSUARIO = "G"; //**C*
            if (!string.IsNullOrEmpty(passwordHash))
            {
                nUser.S_PASSWORD = passwordHash;
            }
            nUser.S_VALIDADOR = user.Validador;
            nUser.S_ESTADO = "V";

            dbSeguridad.Entry(nUser).State = System.Data.Entity.EntityState.Modified;
            dbSeguridad.SaveChanges();

            user.Id = nUser.ID_USUARIO.ToString();

            if (nUser.ID_GRUPO != null)
            {
                // se cargan los roles asociados a su cargo
                var roles = from cr in dbSeguridad.GRUPO_ROL
                            where cr.ID_GRUPO == nUser.ID_GRUPO
                            select cr.ID_ROL;

                foreach (var id in roles)
                {
                    USUARIO_ROL ur = new USUARIO_ROL();
                    ur.ID_USUARIO = nUser.ID_USUARIO;
                    ur.ID_ROL = id;

                    dbSeguridad.USUARIO_ROL.Add(ur);
                }
                if (roles.Count() > 0)
                {
                    dbSeguridad.SaveChanges();
                    //dbSeguridad.SaveChangesAnonymous();
                }
            }
        }

        /// <summary>
        /// Asocia la informacion del login externo a un usuario
        /// </summary>
        /// <param name="idUser">Id del usuario</param>
        /// <param name="loginProvider">nombre del proveedor externo</param>
        /// <param name="providerKey">id que retorna el proveedor externo para identificar el login</param>
        private void CrearUsuarioLogin(int idUser, string loginProvider, string providerKey)
        {
            USUARIO_LOGIN ul = new USUARIO_LOGIN();

            ul.ID_USUARIO = idUser;
            ul.LOGINPROVIDER = loginProvider;
            ul.PROVIDERKEY = providerKey;

            dbSeguridad.USUARIO_LOGIN.Add(ul);
            dbSeguridad.SaveChanges();
            //dbSeguridad.SaveChangesAnonymous();
        }

        /// <summary>
        /// Encriptar la contraseña
        /// </summary>
        /// <param name="pwd">contraseña sin cifrar</param>
        /// <returns>Contraseña cifrada</returns>
        public string EncriptarPassword(string pwd)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "Sha1").ToUpper();
        }

        /// <summary>
        /// Obtiene los valores de la propiedades de la coleccion traida de LDAP
        /// </summary>
        /// <param name="searchResult">juego de propiedades</param>
        /// <param name="PropertyName">nombre de la propiedad</param>
        /// <returns>valor de la propiedad</returns>
        private string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Carga los claims del usuario al usuario de aplicacion
        /// </summary>
        /// <param name="user">usuario</param>
        private void CargarClaimsUsuario(ref ApplicationUser user)
        {
            int idUser = int.Parse(user.Id);
            var dbUser = dbSeguridad.USUARIO.Where(td => td.ID_USUARIO == idUser).First();

            List<int> idRoles = new List<int>();

            // carga los roles asociados al cargo asociado a su tercero
            if (dbUser.PROPIETARIO.Count() > 0)
            {
                foreach (var pp in dbUser.PROPIETARIO.ToList())
                    if (pp.ID_TERCERO.HasValue)
                        user.IdTercero.Add(pp.ID_TERCERO.Value);

                var qrc = from p in dbUser.PROPIETARIO
                          join ct in dbSeguridad.CARGO_TERCERO on p.ID_TERCERO.Value equals ct.ID_TERCERO
                          join cr in dbSeguridad.CARGO_ROL on ct.ID_CARGO equals cr.ID_CARGO
                          where ct.D_FIN >= System.DateTime.Now
                          select cr.ID_ROL;

                if (qrc.Count() > 0)
                    idRoles.AddRange(qrc.ToList());

            }
            else
            {
                //el usuario no se ha creado como tercero, y no se ha registrado el usuario_tercero en la tabla propietario
                //Si es un usuario de BD es porque el usuario no lo ha creado (esto es manual), si es un usuario de DA falta crear el tercero
                //para los usuario DA: es posible en este punto llegar a creaer el tercero y asociarlo al usuario 
                //pero para esto es necesario contar con el numero de documento del usuario, se debe extender el directorio activo
                //para incluir este campo y poder crear 
            }

            //carga los roles asociado a su usuario
            var qru = from r in dbUser.USUARIO_ROL
                      select r.ID_ROL;
            if (qru.Count() > 0)
                idRoles.AddRange(qru.ToList());

            // carga los permisos sobre los controladores de acuerdo a los roles que posee
            var qrf = from r in idRoles.Distinct()
                      join rf in dbSeguridad.ROL_FORMA on r equals rf.ID_ROL
                      select rf;
            user.IdRoles.AddRange(idRoles.Distinct());

            //Pendiente analizar rendimiento de la sesion al cargar todos los permisos, asi como esta,
            //frente a la opcion de cargar solo los idRol y realizar un customAutorize que revise por cada peticion si el rol tiene permiso sobre ese metodo o clase
            foreach (var rol_forma in qrf)
            {
                if (rol_forma.MENU.S_CONTROLADOR == null)
                    continue;
                string contr = rol_forma.MENU.S_CONTROLADOR.ToUpper();
                if (rol_forma.S_NUEVO == "1")
                    user.Permisos.Add("C" + contr);
                if (rol_forma.S_ELIMINAR == "1")
                    user.Permisos.Add("E" + contr);
                if (rol_forma.S_EDITAR == "1")
                    user.Permisos.Add("A" + contr);
                if (rol_forma.S_BUSCAR == "1")
                    user.Permisos.Add("V" + contr);
                if (rol_forma.S_ADMINISTRADOR == "1")
                    user.Permisos.Add("X" + contr);
            }
        }

        /// <summary>
        /// Carga los claims del usuario al objeto ClaimsIdentity
        /// </summary>
        /// <param name="user">usuario</param>
        /// <returns>objeto Identity basado en claims</returns>
        public ClaimsIdentity CreateIdenity(ApplicationUser user)
        {
            ClaimsIdentity id = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
            id.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));

            // agrega claims con el id del usuario
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id, "http://www.w3.org/2001/XMLSchema#string"));
            // agrega claims con el login del usuario
            id.AddClaim(new Claim(ClaimTypes.Name, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));

            // agrega claims con los permisos sobre los controladores que posee el usuario
            foreach (string rol in user.Permisos)
                id.AddClaim(new Claim(ClaimTypes.Role, rol, "http://www.w3.org/2001/XMLSchema#string"));

            // agrega claims con los id de roles asociados al usuario
            foreach (int rol in user.IdRoles)
                id.AddClaim(new Claim(CustomClaimTypes.IdRol, rol.ToString(), "http://www.w3.org/2001/XMLSchema#string"));

            // agrega claims con los id de tercero asociados al usuario
            foreach (int tercero in user.IdTercero)
                id.AddClaim(new Claim(CustomClaimTypes.IdTercero, tercero.ToString(), "http://www.w3.org/2001/XMLSchema#string"));

            return id;
        }

    }
}
 