using Newtonsoft.Json;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class TareaSIMDTO
    {
        [JsonProperty("tareaId")]
        public decimal TareaId { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = string.Empty;
    }
}