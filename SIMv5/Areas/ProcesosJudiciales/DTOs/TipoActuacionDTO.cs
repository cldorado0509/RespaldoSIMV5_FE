using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class TipoActuacionDTO
    {
        [JsonProperty("tipoActuacionId")]
        public int TipoActuacionId { get; set; }

        [JsonProperty("nombre")]
        [StringLength(255)]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        [StringLength(2000)]
        public string Descripcion { get; set; }

        [JsonProperty("activo")]
        [StringLength(1)]
        public string Activo { get; set; }

        [JsonProperty("eliminado")]
        [StringLength(1)]
        public string Eliminado { get; set; }
    }
}
