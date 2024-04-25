using Newtonsoft.Json;

namespace SIM.Areas.Seguridad.Models
{
    public class DatosUsuarioDTO
    {
        [JsonProperty("IdUsuario")]
        public int IdUsuario { get; set; }
        [JsonProperty("IdGrupo")]
        public string IdGrupo { get; set; }
        [JsonProperty("Grupo")]
        public string Grupo { get; set; }
        [JsonProperty("CodFuncionario")]
        public string CodFuncionario { get; set; }
        [JsonProperty("Funcionario")]
        public string Funcionario { get; set; }
        [JsonProperty("Nombres")]
        public string Nombres { get; set; }
        [JsonProperty("Apellidos")]
        public string Apellidos { get; set; }
        [JsonProperty("Login")]
        public string Login { get; set; } = string.Empty;
        [JsonProperty("Email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("Password")]
        public string Password { get; set; } = string.Empty;
        [JsonProperty("FechaRegistro")]
        public string FechaRegistro { get; set; }
        [JsonProperty("FechaVence")]
        public string FechaVence { get; set; }
        [JsonProperty("Tipo")]
        public string Tipo { get; set; }
        [JsonProperty("Estado")]
        public string Estado { get; set; }
        [JsonProperty("IdTercero")]
        public int? IdTercero { get; set; }
        [JsonProperty("DocTercero")]
        public string DocTercero { get; set; } = string.Empty;
        [JsonProperty("NomTercero")]
        public string NomTercero { get; set; } = string.Empty;
    }
}