namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System;
    public class Visita
    {
        [JsonProperty("visitaId")]
        public decimal VisitaId { get; set; }

        [JsonProperty("documentoId")]
        public int DocumentoId { get; set; }

        [JsonProperty("detalleReposicionId")]
        public int DetalleReposicionId { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }

        [JsonProperty("tecnicoId")]
        public int TecnicoId { get; set; }

        [JsonProperty("tecnico")]
        public string Tecnico { get; set; }

        [JsonProperty("acto")]
        public string Acto { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("tipoVisitaId")]
        public int TipoVisitaId { get; set; }

        [JsonProperty("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [JsonProperty("fecha")]
        public DateTime? Fecha { get; set; }

        [JsonProperty("solicitudId")]
        public int SolicitudId { get; set; }
    }
}