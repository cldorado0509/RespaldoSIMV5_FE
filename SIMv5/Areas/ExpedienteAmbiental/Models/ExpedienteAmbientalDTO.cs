 namespace SIM.Areas.ExpedienteAmbiental.Models.DTO
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class ExpedienteAmbientalDTO
    {
        [JsonProperty("idExpediente")]
        public int idExpediente { get; set; }

        [JsonProperty("proyectoId")]
        public int? proyectoId { get; set; }

        [JsonProperty("nombre")]
        [MaxLength(254)]
        public string nombre { get; set; }

        [JsonProperty("terceroId")]
        public int TerceroId { get; set; }

        [JsonProperty("instalacionId")]
        public int InstalacionId { get; set; }


        [JsonProperty("cm")]
        [MaxLength(12)]
        public string cm { get; set; }

        [JsonProperty("descripcion")]
        [MaxLength(254)]
        public string descripcion { get; set; }

        [JsonProperty("clasificacionExpedienteId")]
        public int clasificacionExpedienteId { get; set; }

        [JsonProperty("clasificacionExpediente")]
        public string clasificacionExpediente { get; set; }

        [JsonProperty("municipioId")]
        public int municipioId { get; set; }

        [JsonProperty("municipio")]
        public string municipio { get; set; }

        [JsonProperty("direccion")]
        [MaxLength(500)]
        public string direccion { get; set; }

        [JsonProperty("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; }
    }
}
