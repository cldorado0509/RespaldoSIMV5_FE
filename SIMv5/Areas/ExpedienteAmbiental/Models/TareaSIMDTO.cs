using Newtonsoft.Json;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class TareaSIMDTO
    {
        [JsonProperty("tareaId")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = string.Empty;
    }
}