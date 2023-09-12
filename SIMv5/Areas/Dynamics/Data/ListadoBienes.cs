using Newtonsoft.Json;

namespace SIM.Areas.Dynamics.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ListadoBienes
    {
        [JsonProperty("ASSETID")]
        public string AssetId { get; set; }
        [JsonProperty("ASSETNAME")]
        public string AssetName { get; set; }
        [JsonProperty("ESTADO")]
        public string Estado { get; set; }

    }
}