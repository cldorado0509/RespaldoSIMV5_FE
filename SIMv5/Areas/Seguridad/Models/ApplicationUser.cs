using AspNet.Identity.Oracle;
using Newtonsoft.Json;
using System.Collections.Generic;

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
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("TipoUsuario")]
        public int? TipoUsuario { get; set; }
        [JsonProperty("TipoPersonaUsuario")]
        public string TipoPersonaUsuario { get; set; }
        [JsonProperty("Validador")]
        public string Validador { get; set; }
        [JsonProperty("Recuperacion")]
        public string Recuperacion { get; set; }
        [JsonProperty("isAuthenticated")]
        public bool isAuthenticated { get; set; }
        [JsonProperty("errAuthenticate")]
        public string errAuthenticate { get; set; }
        [JsonProperty("IdTercero")]
        public List<int> IdTercero { get; set; }
        [JsonProperty("Permisos")]
        public List<string> Permisos { get; set; }
        [JsonProperty("IdRoles")]
        public List<int> IdRoles { get; set; }
        [JsonProperty("Token")]
        public string Token { get; set; }
        [JsonProperty("ExpiresIn")]
        public int ExpiresIn { get; set; }
        public byte[] Foto { get; set; }
    }
}