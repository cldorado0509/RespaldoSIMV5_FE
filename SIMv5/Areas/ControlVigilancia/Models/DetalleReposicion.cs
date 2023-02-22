namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class DetalleReposicion
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("reposicionId")]
        public int ReposicionId { get; set; }

        [JsonProperty("tipoDocumento")]
        public int TipoDocumento { get; set; }

        [JsonProperty("documentoId")]
        public int DocumentoId { get; set; }

        [JsonProperty("anioRadicadoVisita")]
        public int AnioRadicadoVisita { get; set; }

        [JsonProperty("tramiteId")]
        public string TramiteId { get; set; }

        [JsonProperty("numeroActo")]
        public string NumeroActo { get; set; }

        [JsonProperty("fechaActo")]
        public DateTime FechaActo { get; set; }

        [JsonProperty("anioActo")]
        public int AnioActo { get; set; }

        [JsonProperty("talaEjecutada", Required = Required.Always)]
        [Required]
        public int TalaEjecutada { get; set; }

        [JsonProperty("dAPMen10Ejecutada", Required = Required.Always)]
        [Required]
        public int DAPMen10Ejecutada { get; set; }

        [JsonProperty("volumenEjecutado", Required = Required.Always)]
        [Required]
        public float VolumenEjecutado { get; set; }

        [JsonProperty("transplanteEjecutado", Required = Required.Always)]
        [Required]
        public int TransplanteEjecutado { get; set; }

        [JsonProperty("podaEjecutada", Required = Required.Always)]
        [Required]
        public int PodaEjecutada { get; set; }

        [JsonProperty("conservacionEjecutada", Required = Required.Always)]
        [Required]
        public int ConservacionEjecutada { get; set; }

        [JsonProperty("reposicionEjecutada", Required = Required.Always)]
        [Required]
        public int ReposicionEjecutada { get; set; }

        [JsonProperty("medidaAdicionalEjecutada", Required = Required.Always)]
        [Required]
        public float MedidaAdicionalEjecutada { get; set; }

        [JsonProperty("fechaControl")]
        public DateTime FechaControl { get; set; }

        [JsonProperty("observacionVisita")]
        public string ObservacionVisita { get; set; }

        [JsonProperty("fechaVisita")]
        public DateTime? FechaVisita { get; set; }

        [JsonProperty("tecnicoId")]
        public int? TecnicoId { get; set; }

        [JsonProperty("radicadoVisita")]
        public string RadicadoVisita { get; set; }

        [JsonProperty("fechaRadicadoVisita")]
        public DateTime? FechaRadicadoVisita { get; set; }
        

    }
}