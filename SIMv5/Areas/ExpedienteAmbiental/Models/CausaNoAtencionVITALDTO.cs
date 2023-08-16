using Newtonsoft.Json;
namespace SIM.Areas.ExpedienteAmbiental.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CausaNoAtencionVITALDTO
    {
        [JsonProperty("causaNoAtencionVITALId")]
        public decimal CausaNoAtencionVITALId { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonProperty("habilitado")]
        public string Habilitado { get; set; } = string.Empty;
    }
}