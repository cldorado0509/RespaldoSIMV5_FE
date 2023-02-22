namespace SIM.Areas.ParqueAguas.Models
{
    using Newtonsoft.Json;
    using System;
    public class AforoDisponible
    {
        
        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        [JsonProperty("aforo")]
        public int Aforo { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}