namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class TipoSolicitudAmbientalDTO
    {
        [JsonProperty("idTipoSolicitudAmbiental")] 
        public int IdTipoSolicitudAmbiental { get; set; }

        [JsonProperty("idMigracion")]
        public int IdMigracion { get; set; }

        [JsonProperty("componenteAmbientalId")]
        public int ComponenteAmbientalId { get; set; }

        /// <summary>
        /// Nombre del tipo de solicitud ambiental
        /// </summary>
        [JsonProperty("nombre")]
        [MaxLength(254)]
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del tipo de solicitud ambiental
        /// </summary>
        [JsonProperty("descripcion")]
        [MaxLength(2000)]
        public string Descripcion { get; set; }

        /// <summary>
        ///  Establece si el tipo de solicitud ambiental requiere de auto de inicio
        /// </summary>
        [JsonProperty("requiereAutoInicio")]
        public bool RequiereAutoInicio { get; set; }

        /// <summary>
        /// Establece si el tipo de solicitud ambiental requiere de resolución
        /// </summary>
        [JsonProperty("requiereResolucion")]
        public bool RequiereResolucion { get; set; }

        /// <summary>
        /// Establece si el tipo de solicitud ambiental se encuentra habilitado
        /// </summary>
        [JsonProperty("habilitado")]
        public bool Habilitado { get; set; }
    }
}