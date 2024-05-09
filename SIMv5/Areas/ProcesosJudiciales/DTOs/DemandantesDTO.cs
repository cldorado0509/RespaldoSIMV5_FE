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
        public string demantanteId { get; set; }

        [JsonProperty("identificacion")]
        public string Identificacion { get; set; } = String.Empty;

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = String.Empty;
    }
}