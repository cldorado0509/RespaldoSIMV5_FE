
using Newtonsoft.Json;
using System;

namespace SIM.Areas.ControlVigilancia.Models
{
    public class DatoRadicado
    {
        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("fecharadicado")]
        public int FechaRadicado { get; set; }

        [JsonProperty("dfecharadicado")]
        public DateTime DFechaRadicado { get; set; }
    }
}