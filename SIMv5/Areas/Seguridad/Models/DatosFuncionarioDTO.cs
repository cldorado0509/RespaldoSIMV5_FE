using Newtonsoft.Json;

namespace SIM.Areas.Seguridad.Models
{
    public class DatosFuncionarioDTO
    {
        [JsonProperty("CodFuncionario")]
        public int CodFuncionario { get; set; }
        [JsonProperty("Cedula")]
        public string Cedula { get; set; }
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }
        [JsonProperty("Apellido")]
        public string Apellido { get; set; }
        [JsonProperty("Cargo")]
        public string Cargo { get; set; }
        [JsonProperty("GrupoTarbajo")]
        public string GrupoTarbajo { get; set; }
        [JsonProperty("Oficina")]
        public string Oficina { get; set; }
        [JsonProperty("Extension")]
        public string Extension { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; } = string.Empty;
        [JsonProperty("Estado")]
        public string Estado { get; set; } = string.Empty;

        [JsonProperty("FirmaDigital")]
        public string FirmaDigital { get; set; } = string.Empty;
        [JsonProperty("UsrFirmaDigital")]
        public string UsrFirmaDigital { get; set; }
        [JsonProperty("FechaFirmaDigital")]
        public string FechaFirmaDigital { get; set; }
        [JsonProperty("PoseeFirma")]
        public string PoseeFirma { get; set; }
    }
}