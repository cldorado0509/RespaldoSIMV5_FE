namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class RegistroControl
    {
        public int Id { get; set; }

        [JsonProperty("reposicionId", Required = Required.Always)]
        [Required]
        public int ReposicionId { get; set; }

        [JsonProperty("tipoDocumentoId", Required = Required.Always)]
        [Required]
        public int TipoDocumentoId { get; set; }
       
        [JsonProperty("documentoId", Required = Required.Always)]
        [Required]
        public int DocumentoId { get; set; }

        [JsonProperty("numeroActo", Required = Required.Always)]
        [Required]
        public string NumeroActo { get; set; }

        [JsonProperty("fechaActo", Required = Required.Always)]
        [Required]
        public DateTime FechaActo { get; set; }

        [JsonProperty("anioActo", Required = Required.Always)]
        [Required]
        public int AnioActo { get; set; }


        [JsonProperty("talaEjecutada", Required = Required.Always)]
        [Required]
        public int TalaEjecutada { get; set; }

        [JsonProperty("dapMen10Ejecutada", Required = Required.Always)]
        [Required]
        public int DapMen10Ejecutada { get; set; }

        [JsonProperty("volumenEjecutado", Required = Required.Always)]
        [Required]
        public float VolumenEjecutado { get; set; }

        [JsonProperty("trasplanteEjecutado", Required = Required.Always)]
        [Required]
        public int TrasplanteEjecutado { get; set; }

        [JsonProperty("podaEjecutada", Required = Required.Always)]
        [Required]
        public int PodaEjecutada { get; set; }

        [JsonProperty("conservacionEjecutada", Required = Required.Always)]
        [Required]
        public int ConservacionEjecutada { get; set; }

        [JsonProperty("medidaAdicionalEjecutada", Required = Required.Always)]
        [Required]
        public float MedidaAdicionalEjecutada { get; set; }

        [JsonProperty("fechaControl", Required = Required.Always)]
        [Required]
        public DateTime FechaControl { get; set; }
    }
}