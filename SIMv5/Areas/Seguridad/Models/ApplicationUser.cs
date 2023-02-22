using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.Oracle;

namespace SIM.Areas.Seguridad.Models
{
    /// <summary>
    /// Modelo de usuario de la aplicacion
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName)
            : base(userName)
        {
            isAuthenticated = false;
            IdTercero = new List<int>();
            Permisos = new List<string>();
            IdRoles = new List<int>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? TipoUsuario { get; set; }
        public string TipoPersonaUsuario { get; set; }
        public string Validador { get; set; }
        public string Recuperacion { get; set; }

        public bool isAuthenticated { get; set; }
        public string errAuthenticate { get; set; }
        public List<int> IdTercero { get; set; }
        public List<string> Permisos { get; set; }
        public List<int> IdRoles { get; set; }
    }
}