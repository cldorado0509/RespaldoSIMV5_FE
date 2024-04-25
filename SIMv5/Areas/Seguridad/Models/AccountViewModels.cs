using System;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.Seguridad.Models
{
    /// <summary>
    /// Modelo de datos para la vista de registro de usuarios con autenticacion en un proveedor externo
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        //Nuevos campos para extender el modelo de registro del usuario

        [Required]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Tipo de usuario")]
        public int? TipoUsuario { get; set; }

        [Required]
        [Display(Name = "Tipo de Persona Usuario")]
        public string TipoPersonaUsuario { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser(this.UserName)
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                TipoUsuario = this.TipoUsuario,
            };
            return user;
        }
    }

    /// <summary>
    /// Modelo de datos para la vista de administracion de usuario, donde cada usuario puede cambiar su contraseña
    /// </summary>
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} debe ser mayor a {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La contraseña no coincide con la confirmación.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Modelo de datos para la vista de inicio de sesion de usuarios
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordar me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Modelo de datos para la vista de registro de nuevos usuarios
    /// </summary>
    public class RegisterViewModel
    {
        //Nuevos campos para extender el modelo de registro del usuario

        [Required]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Tipo de usuario")]
        public int? TipoUsuario { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Tipo de Persona")]
        public string TipoPersonaUsuario { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} debe ser mayor a {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide con la confirmación.")]
        public string ConfirmPassword { get; set; }

        // Tipo de invocación: 0 Verificación de datos, 1 Administrador Empresa, 2 Roles Empresa
        public int Type { get; set; }

        public string Roles { get; set; }
        public string RolesNombres { get; set; }

        public int NumFiles { get; set; }

        public string Hash { get; set; }

        public string Nit { get; set; }

        public string RazonSocial { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser(this.UserName)
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                TipoUsuario = this.TipoUsuario,
                TipoPersonaUsuario = this.TipoPersonaUsuario,
                Validador = Utilidades.Cryptografia.GetSHA1(this.Email + "^" + this.FirstName.ToUpper() + this.LastName.ToUpper()).ToUpper(),
                Recuperacion = Utilidades.Cryptografia.GetSHA1(this.Email + "^" + this.FirstName.ToUpper() + this.LastName.ToUpper()).ToUpper() + "|" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMddHHmmss")))
            };
            return user;
        }
    }
}