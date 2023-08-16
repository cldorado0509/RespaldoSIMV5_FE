using Newtonsoft.Json;

namespace SIM.Areas.Dynamics.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ListadoTercero
    {
        [JsonProperty("DOCUMENTO")]
        public string Tercero { get; set; }
        [JsonProperty("TERCERO")]
        public string Name { get; set; }
        [JsonProperty("AGNO")]
        public int Agno { get; set; }
    }
}