namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class TipoEstadoPuntoControlDTO
    {
        [JsonProperty("idTipoEstadoPuntoControl")]
        public int IdTipoEstadoPuntoControl { get; set; }

        [JsonProperty("idMigracion")]
        public int IdMigracion { get; set; }

        /// <summary>
        ///Nombre del Tipo de Estado del Punto de Control
        /// </summary>
        [JsonProperty("nombre")]
        [MaxLength(254)]
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del tipo de estado del Punto de Control
        /// </summary>
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Establece si el tipo de estado del Punto de Control se encuentra habilitado
        /// </summary>
        [JsonProperty("habilitado")]
        public bool Habilitado { get; set; }
    }
}