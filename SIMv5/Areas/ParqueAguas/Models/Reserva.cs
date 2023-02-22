namespace SIM.Areas.ParqueAguas.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    public class Reserva
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }

        [JsonProperty("numeroComprobante")]
        public string NumeroComprobante { get; set; }

        [JsonProperty("valor")]
        public long Valor { get; set; }

        [JsonProperty("nroVisitantes")]
        public int NroVisitantes { get; set; }

        [JsonProperty("pos")]
        public string Pos { get; set; }

        [JsonProperty("visitantes")]
        public List<Visitante> Visitantes { get; set; }

    }
}