using Newtonsoft.Json;

namespace SIM.Areas.Dynamics.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class TercerosModel
    {
        [JsonProperty("TERCERO")]
        public string Tercero { get; set; }

        [JsonProperty("NOMBRE")]
        public string Name { get; set; }
    }
}