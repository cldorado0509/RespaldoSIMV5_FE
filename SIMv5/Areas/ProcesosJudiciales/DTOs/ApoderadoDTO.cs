using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class ApoderadoDTO
    {
        [JsonProperty("apoderadoId")]
        public int ApoderadoId { get; set; }

        [JsonProperty("identificacion")]
        public string Identificacion { get; set; } = String.Empty;

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = String.Empty;
    }
}