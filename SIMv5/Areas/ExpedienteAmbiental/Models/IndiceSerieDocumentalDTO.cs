namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class IndiceSerieDocumentalDTO
    {
        [JsonProperty("indiceSerieDocumentaId")]
        public int IndiceSerieDocumentaId { get; set; }

        [JsonProperty("valorString")]
        public string ValorString { get; set; } = string.Empty;

        [JsonProperty("valorNumerico")]
        public decimal? ValorNumerico { get; set; }

        [JsonProperty("valorFecha")]
        public DateTime? ValorFecha { get; set; }


    }
}