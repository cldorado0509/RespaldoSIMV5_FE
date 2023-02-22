namespace SIM.Areas.ExpedienteAmbiental.Models.DTO
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class ClasificacionExpedienteDTO
    {
        [JsonProperty("idClasificacionExpediente")]
        public int IdClasificacionExpediente { get; set; }

        /// <summary>
        /// Descripción o nombre del tipo de clasificación del expediente ambiental
        /// </summary>

        [JsonProperty("nombre")]
        [MaxLength(254)]
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Establece si el tipo de clasificación del expediente ambiental se encuentra habilitado
        /// </summary>
        [JsonProperty("habilitado")]
        public bool Habilitado { get; set; }

        [JsonProperty("idMigracion")]
        public int? IdMigracion { get; set; }
    }
}