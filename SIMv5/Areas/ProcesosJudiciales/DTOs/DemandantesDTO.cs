using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    public class DemandantesDTO
    {
        [JsonProperty("demantanteId")]
        public int DemantanteId { get; set; }

        [JsonProperty("identificacion")]
        public string Identificacion { get; set; } = String.Empty;

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = String.Empty;

        [JsonProperty("isNew")]
        public string IsNew { get; set; }

        [JsonProperty("esConvocante")]
        public string EsConvocante { get; set; } = "1";

        [JsonProperty("esDemandante")]
        public string EsDemandante { get; set; } = "1";
    }
}