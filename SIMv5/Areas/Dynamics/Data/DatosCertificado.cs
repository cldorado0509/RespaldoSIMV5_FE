using Newtonsoft.Json;

namespace SIM.Areas.Dynamics.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DatosCertificado
    {
        [JsonProperty("AGNO")]
        public int AGNO { get; set; }
        [JsonProperty("TERCERO")]
        public string TERCERO { get; set; } = string.Empty;
        [JsonProperty("CUENTAPRINCIPAL")]
        public string CUENTAPRINCIPAL { get; set; } = string.Empty;
        [JsonProperty("VALOR")]
        public long VALOR { get; set; }
        [JsonProperty("OPERACION")]
        public string OPERACION { get; set; } = string.Empty;
    }
}