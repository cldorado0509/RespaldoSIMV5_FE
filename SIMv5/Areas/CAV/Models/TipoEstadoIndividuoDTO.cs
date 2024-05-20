using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{
    public class TipoEstadoIndividuoDTO
    {
        /// <summary>
        /// Identifica el Tipo de Estado del Individuo
        /// </summary>
        [JsonProperty("tipoEstadoIndividuoId")]
        public decimal TipoEstadoIndividuoId { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Display(Name = "Nombre")]
        [MaxLength(1000, ErrorMessage = "NombreMaxLength")]
        [Required(ErrorMessage = "NombreRequired")]
        [JsonProperty("nombre")]
        public string Nombre { get; set; } = string.Empty;


        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "Descripción")]
        [MaxLength(4000, ErrorMessage = "DescripcionMaxLength")]
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Eliminado
        /// </summary>
        [Display(Name = "Eliminado")]
        [MaxLength(1, ErrorMessage = "EliminadoMaxLength")]
        [JsonProperty("eliminado")]
        public string Eliminado { get; set; }

        /// <summary>
        /// Activo
        /// </summary>
        [Display(Name = "Activo")]
        [MaxLength(1, ErrorMessage = "ActivoMaxLength")]
        [JsonProperty("activo")]
        public string Activo { get; set; }
    }
}
