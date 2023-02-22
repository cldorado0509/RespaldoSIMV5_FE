namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Asunto
    {
        [JsonProperty("Id")]
        public decimal Id { get; set; }

        [JsonProperty("TramiteId")]
        public decimal TramiteId { get; set; }

        [JsonProperty("DocumentoId")]
        public decimal DocumentoId { get; set; }

        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        [JsonProperty("Solicitante")]
        public string Solicitante { get; set; }

        [JsonProperty("TipoSolicitud")]
        public string TipoSolicitud { get; set; }

        [JsonProperty("SubTipoSolicitud")]
        public string SubTipoSolicitud { get; set; }

        [JsonProperty("Radicado")]
        public string Radicado { get; set; }

        [JsonProperty("Anio")]
        public string Anio { get; set; }

        [JsonProperty("Asunto")]
        public string Asunto_ { get; set; }

        [JsonProperty("Proyecto")]
        public string Proyecto { get; set; }

        [JsonProperty("Cm")]
        public string CM { get; set; }

        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("FechaRegistro")]
        public DateTime FechaRegistro { get; set; }
    }
}