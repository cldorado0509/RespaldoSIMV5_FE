using Newtonsoft.Json;
using System;

namespace SIM.Areas.Dynamics.Data
{
    public class DatosTerceroModel
    {
        [JsonProperty("TERCERO")]
        public string TERCERO { get; set; } = string.Empty;
        [JsonProperty("NOMBRE")]
        public string NOMBRE { get; set; } = string.Empty;
        [JsonProperty("PRIMERNOMBRE")]
        public string FIRSTNAME { get; set; } = string.Empty;
        [JsonProperty("SEGUNDONOMBRE")]
        public string MIDDLENAME { get; set; } = string.Empty;
        [JsonProperty("APELLIDO")]
        public string LASTNAME { get; set; } = String.Empty;
        [JsonProperty("SEGUNDOAPELLIDO")]
        public string AP_CO_SECONDLASTNAME { get; set; } = string.Empty;
        [JsonProperty("CUENTAPRINCIPAL")]
        public string CUENTAPRINCIPAL { get; set; } = string.Empty;
        [JsonProperty("VALORRETENCION")]
        public long VALORRETENCION { get; set; }
        [JsonProperty("VALORINGRESO")]
        public long VALORINGRESO { get; set; }
        [JsonProperty("AGNO")]
        public int AGNO { get; set; }
    }
}