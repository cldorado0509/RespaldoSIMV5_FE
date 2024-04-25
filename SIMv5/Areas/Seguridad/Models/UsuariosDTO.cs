using Newtonsoft.Json;
using System;

namespace SIM.Areas.Seguridad.Models
{
    public class UsuariosDTO
    {
        [JsonProperty("ID_USUARIO")]
        public int ID_USUARIO { get; set; }
        [JsonProperty("GRUPO")]
        public string GRUPO { get; set; }
        [JsonProperty("S_LOGIN")]
        public string S_LOGIN { get; set; }
        [JsonProperty("NOMBRE")]
        public string NOMBRE { get; set; }
        [JsonProperty("FUNCIONARIO")]
        public string FUNCIONARIO { get; set; }
        [JsonProperty("VENCE")]
        public DateTime? VENCE { get; set; }
        [JsonProperty("ESTADO")]
        public string ESTADO { get; set; }


    }
}
