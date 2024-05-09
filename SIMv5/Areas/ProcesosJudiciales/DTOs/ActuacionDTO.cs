using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class ActuacionDTO
    {
        [JsonProperty("actuacionId")]
        public int ActuacionId { get; set; }

        [JsonProperty("etapa")]
        public string Etapa { get; set; } = String.Empty;

        [JsonProperty("Concilición")]
        public bool Conciliacion { get; set; }

        [JsonProperty("valorConciliado")]
        public float ValorConciliado { get; set; }

        [JsonProperty("finalizado")]
        public bool Finalizado { get; set; }

        [JsonProperty("fechaFinalizacion")]
        public DateTime FechaFinalizacion { get; set; }

        [JsonProperty("comiteVerificacion")]
        public bool ComiteVerificacion { get; set; }

        [JsonProperty("desacato")]
        public bool Desacato { get; set; }

    }
}