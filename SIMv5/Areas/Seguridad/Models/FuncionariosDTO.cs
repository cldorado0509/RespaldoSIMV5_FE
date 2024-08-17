using Newtonsoft.Json;

namespace SIM.Areas.Seguridad.Models
{
    public class FuncionariosDTO
    {
        [JsonProperty("CODFUNCIONARIO")]
        public int CODFUNCIONARIO { get; set; }

        [JsonProperty("NOMBRES")]
        public string NOMBRES { get; set; }

        [JsonProperty("APELLIDOS")]
        public string APELLIDOS { get; set; }

        [JsonProperty("CARGO")]
        public string CARGO { get; set; }

        [JsonProperty("OFICINA")]
        public string OFICINA { get; set; }

        [JsonProperty("EXTENSION")]
        public string EXTENSION { get; set; }

        [JsonProperty("EMAIL")]
        public string EMAIL { get; set; }

        [JsonProperty("GRUPOTRABAJO")]
        public string GRUPOTRABAJO { get; set; }

        [JsonProperty("ACTIVO")]
        public string ACTIVO { get; set; }

        [JsonProperty("CEDULA")]
        public string CEDULA { get; set; }
    }
}
