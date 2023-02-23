using Newtonsoft.Json;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class DocumentosRequeridosTramitesDTO
    {

        [JsonProperty("documentosRequeridosTramitesId")]
        public int DocumentosRequeridosTramitesId { get; set; }

        [JsonProperty("orden")]
        public int Orden { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("claseAtencionId")]
        public int? ClaseAtencionId { get; set; }

        [JsonProperty("formato")]
        public string Formato { get; set; }

        [JsonProperty("obligatorio")]
        public string Obligatorio { get; set; } = string.Empty;
    }
}