
namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class UsuarioDTO
    {
        [JsonProperty("idUsuario")]
        public decimal IdUsuario { get; set; }

        [JsonProperty("idGrupo")]
        public decimal? IdGrupo { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("passwordOld")]
        public string PasswordOld { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("superUsuario")]
        public string SuperUsuario { get; set; }

        [JsonProperty("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; }

        [JsonProperty("fechaVence")]
        public DateTime? FechaVence { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("saltos")]
        public decimal? Saltos { get; set; }

        [JsonProperty("validador")]
        public string Validador { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }
    }
}