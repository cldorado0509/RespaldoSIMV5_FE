using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class DemandadosDTO
    {
        [JsonProperty("demandadoId")]
        public int DemandadoId { get; set; }

        [JsonProperty("identificacion")]
        public string Identificacion { get; set; } = String.Empty;

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = String.Empty;

        [JsonProperty("isNew")]
        public string IsNew { get; set; }

        [JsonProperty("esConvocado")]
        public string EsConvocado { get; set; } = "1";

        [JsonProperty("esDemandado")]
        public string EsDemandado { get; set; } = "1";
    }
}