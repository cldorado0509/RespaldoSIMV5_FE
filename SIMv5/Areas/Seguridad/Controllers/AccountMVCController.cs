using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AspNet.Identity.Oracle;
using Microsoft.Owin.Security;
using SIM.Areas.Seguridad.Models;
using SIM.Data;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// Controlador Account: Gestiona el inicio de sesión de usuario en la aplicación
    /// </summary>
    [Authorize]
    public class AccountMVCController : Controller
    {
        //Se crea objeto IdentityManager para administrar los metodos de busqueda, creacion y administracion de un usuario y su sesion
        private IdentityManager _idManager = new IdentityManager();
        private EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();

        public AccountMVCController()
            : this(new UserManager<IdentityUser>(new UserStore<IdentityUser>(new OracleDatabase())))
        {
        }

        public AccountMVCController(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<IdentityUser> UserManager { get; private set; }


        /// <summary>
        /// Retorna la vista mediante la cual un usuario puede iniciar sesión en la aplicación. LLamda GET a la acción.
        /// </summary>
        /// <param name="returnUrl">url de retorno donde se redireccionará al usuario luego de iniciar sesión</param>
        /// <returns>Vista para iniciar sesión </returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        /// <summary>
        /// Gestiona la autenticación del usuario cuando este pertenece a la base de datos o directorio activo, e inicia sesión en la aplicación. LLamda POST a la acción.
        /// </summary>
        /// <param name="model">modelo con la información del usuario para iniciar sesión</param>
        /// <param name="returnUrl">url de retorno donde se redireccionará al usuario luego de iniciar sesión</param>
        /// <returns>Vista home del SIM o vista para iniciar sesión en caso de error</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Valida si el usuario existe en base de datos o en el directorio activo                
                var user = _idManager.FindUser(model.UserName, model.Password);
                if (user.isAuthenticated)
                {
                    //Se crea la sesión del usuario
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", user.errAuthenticate);
                }
            }

            //Si existe algún se retorna a la vista de inicio de sesión y se despliega el error obtenido
            return View(model);
        }


        /// <summary>
        /// Retorna la vista para que los usuarios se puedan registrar en la aplicación. Llamada GET a la acción.
        /// </summary>
        /// <returns>Vista para registrar nuevo usuario </returns>r
        [AllowAnonymous]
        public ActionResult Register()
        {
            //Carga la variable tipousuario con los grupos activos y vigentes para que el usuario seleccione el perfil que desempeñará en la aplicacion
            ViewBag.TIPOUSUARIO = new SelectList(dbSeguridad.GRUPO.Where(td => td.D_FIN >= System.DateTime.Now && td.S_VISIBLE=="1"), "ID_GRUPO", "S_NOMBRE");
            return View();
        }


        /// <summary>
        /// Valida la información de registro del usuario, si es correcta lo registra en la aplicación e inicia sesión. LLamda POST a la acción.
        /// </summary>
        /// <param name="model">modelo con la información del usuario para registrarse en la aplicación</param>
        /// <returns>Vista home del SIM o vista para registrar usuario en de caso de error </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string tipoPersonaUsuario)
        {
            if (ModelState.IsValid)
            {
                //Se registra el usuario en base de datos
                var user = _idManager.RegisterUserBD(model.GetUser(), model.Password);
                if (user.isAuthenticated)
                {
                    // se crea la sesión del usuario registrado
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    ModelState.AddModelError("", user.errAuthenticate);
                }

            }

            ViewBag.TipoPersonaUsuario = tipoPersonaUsuario;

            // Si hay error se retorna la vista para el registro del usuario con los errores ocurridos
            //Carga la variable tipousuario con los grupos activos y vigentes para que el usuario seleccione el perfil que desempeñará en la aplicacion
            ViewBag.TIPOUSUARIO = new SelectList(dbSeguridad.GRUPO.Where(td => td.D_FIN >= System.DateTime.Now && td.S_VISIBLE == "1"), "ID_GRUPO", "S_NOMBRE");
            return View(model);
        }

        /// <summary>
        /// Retorna la vista mediante la cual el usuario administra su cuenta, allí puede cambiar su contraseña. LLamda GET a la acción.
        /// </summary>
        /// <param name="message">identificador del tipo de mensaje resultao de la operación de administrar la cuenta del usuario</param>
        /// <returns>Vista de administracion de la cuenta de usuario</returns>
        public ActionResult Manage(ManageMessageId? message)
        {
            //Se setea la descripcion del mensaje a mostrar de acuerdo al identificador obtenido como resultado de la operacion efectuada
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Properties.ResourcesMensajes.msjContrasenaActualizada
                : message == ManageMessageId.SetPasswordSuccess ? Properties.ResourcesMensajes.msjContrasenaSeteada
                : message == ManageMessageId.RemoveLoginSuccess ? Properties.ResourcesMensajes.msjLoginRemovido
                : message == ManageMessageId.Error ? Properties.ResourcesError.errInesperado
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }


        /// <summary>
        /// Retorna la vista mediante la cual el usuario administra su cuenta, allí puede cambiar su contraseña. LLamda POST a la acción.
        /// </summary>
        /// <param name="message">modelo con la información a gestionar de la cuenta del usuario</param>
        /// <returns>Vista de administracion de la cuenta de usuario</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    string result = _idManager.ChangePasswordUserBD(int.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
                    if (string.IsNullOrEmpty(result))
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    else
                    {
                        ModelState.AddModelError("", result);
                    }

                }
            }

            // Si hay error se retorna la vista para la administracion de la cuenta de usuario con los errores ocurridos
            return View(model);
        }
        
        /// <summary>
        /// Es llamado cuando se invoca la autenticacion para un provider externo. LLamda POST a la acción.
        /// </summary>
        /// <param name="provider">provider seleccionado en el que se hará la autenticación</param>
        /// <param name="returnUrl">url de retorno despues de autenticar el usuario</param>
        /// <returns>Objeto ChallengeResult que representa el resultado de un unauthorized HTTP requestreturns</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Redirecciona hacia el proveedor de acceso externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        /// <summary>
        /// Valida la informacion retornada por el proveedor externo y si es correcta inicia la sesion del usuario en el SIM. LLamda GET a la acción.
        /// </summary>
        /// <param name="returnUrl">url de retorno despues de autenticar el usuario</param>
        /// <returns>
        ///         Vista Login en caso de no ser autenticado. 
        ///         Vista ExternalLoginConfirmation en caso que la autenticacion es correcta pero el usuario no exista en el SIM
        ///         Redireccionado a la url donde quería acceder el usuario en caso de que sea correcta la autenticacion y ya se encuentre registrado en el SIM
        /// </returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            //obtiene el resultado de la autenticacion del usuario con el proveedor externo
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            //Si no fue posible autenticarse contra proveedor externo se retorna a vista login
            if (loginInfo == null)
            {
                ModelState.AddModelError("", Properties.ResourcesError.errProveedorAutenticacionNoDisponible);
                return View("Login");
            }

            // Se verifica si el usuario ya esta registrado en el SIM con ese proveedor externo, si si se inicia sesion,
            // si no es redireccionado para que se registre en el SIM con base a la informacion retornada por el proveedor externo.
            var user = _idManager.FindUser(loginInfo.Login);      
            // Usuario ya registrado y esta activo
            if (user.isAuthenticated)
            {
                // se crea la sesión del usuario
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {   
                // Usuario no registrado, se crea una cuenta de usuario en la aplicacion
                if (string.IsNullOrEmpty(user.errAuthenticate))
                {
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    // Se obtienen los claims retornados por el proveedor externo para crear la cuenta en el SIM con dicha información
                    ExternalLoginConfirmationViewModel elcvm = new ExternalLoginConfirmationViewModel();
                    var externalIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

                    elcvm.FirstName = externalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                    elcvm.LastName = externalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                    elcvm.UserName = externalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                    elcvm.Email = externalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                    //Carga la variable tipousuario con los grupos activos y vigentes para que el usuario seleccione el perfil que desempeñará en la aplicacion
                    ViewBag.TIPOUSUARIO = new SelectList(dbSeguridad.GRUPO.Where(td => td.D_FIN >= System.DateTime.Now && td.S_VISIBLE == "1"), "ID_GRUPO", "S_NOMBRE");

                    return View("ExternalLoginConfirmation", elcvm);
                }
                else
                {
                    // Usuario ya esta registrado pero esta inactivo
                    ModelState.AddModelError("", user.errAuthenticate);
                    return View("Login");
                }
            }
        }

        /// <summary>
        /// Permite registrar un usuario autenticado con un proveedor externo. LLamda POST a la acción.
        /// </summary>
        /// <param name="model">modelo con la informacion del usuario</param>
        /// <param name="returnUrl">url de retorno despues de registrar el usuario</param>
        /// <returns>
        ///         Vista Manage en caso de que el usuario ya tenga sesion activa en el SIM. 
        ///         Vista ExternalLoginFailure en caso que no sea posible obtener la informacion del usuario retornado por el proveedor externo
        ///         Redireccionado a la url donde quería acceder el usuario en caso de que el registro en el SIM sea correcto
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            // se valida si ya ha iniciado sesion en la aplicacion
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // obtiene la informacion del usuario retornada por el proveedor externo
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                //Si no fue posible obtener la informacion del proveedor externo se retorna a vista ExternalLoginFailure
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                // Se registra el usuario autenticado con el proveedor externo en la aplicacion
                var user = _idManager.RegisterUserWeb(model.GetUser(), info.Login);
                if (user.isAuthenticated)
                {
                    // se crea la sesión del usuario
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", user.errAuthenticate);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Cierra sesion de usuario en la aplicacion. LLamda POST a la acción.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session["_MenuPpal"] = null;
            Session["_Menu"] = null;
            return RedirectToAction("Index", "Home");
        }
        
        /// <summary>
        /// Retorna vista de error de inicio de sesion con el proveedor externo. LLamda GET a la acción.
        /// </summary>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region MetodosNativosNoImplementados
        
        /// <summary>
        /// Permite eliminar un login externo (google, facebook, etc) de la aplicación. Llamada POST
        /// Toca adecuarlo si se ha de implementar en el SIM.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        /// <summary>
        /// Redirecciona al proveedor externo para vincular un inicio de sesion del usuario actual. Llamada POST
        /// Toca adecuarlo si se ha de implementar en el SIM.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }
        
        /// <summary>
        /// Permite asociar login de proveedor externo al inicio de sesion actual del usuario. Llamada GET
        /// Toca adecuarlo si se ha de implementar en el SIM.
        /// </summary>
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
       

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Inicia sesion de usuario en la aplicacion
        /// </summary>
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            Session["_MenuPpal"] = null;
            var identity = _idManager.CreateIdenity(user);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        /// <summary>
        /// Permite adicionar mensajes de errores al modelo de estados de un objeto
        /// </summary>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// Permite saber si el usuario almacena o no su contraseña en la aplicacion
        /// </summary>
        private bool HasPassword()
        {
            return _idManager.UserHasPassword(int.Parse(User.Identity.GetUserId()));
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }


        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}