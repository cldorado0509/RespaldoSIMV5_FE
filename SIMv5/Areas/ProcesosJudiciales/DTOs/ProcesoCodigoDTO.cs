using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class ProcesoCodigoDTO
    {
        [JsonProperty("procesoCodigoId")]
        public int ProcesoCodigoId { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; } = String.Empty;
    }
}