namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class SeguimientoTramiteNuevo
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("reposicionId")]
        public int ReposicionId { get; set; }

        [JsonProperty("fechaControl")]
        public DateTime FechaControl { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("fechaRadicado")]
        public int FechaRadicado { get; set; }

        [JsonProperty("anioRadicado")]
        public int AnioRadicado { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("descripcionEstado")]
        public string DescripcionEstado { get; set; }

        [JsonProperty("tecnico")]
        public string Tecnico { get; set; }

        [JsonProperty("tramiteId")]
        public int TramiteId { get; set; }

        [JsonProperty("documentoId")]
        public int DocumentoId { get; set; }

    }
}