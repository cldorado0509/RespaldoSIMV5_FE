using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{

    /// <summary>
    /// DTO Tipo de Adquisición
    /// </summary>
    public class TipoAdquisicionDTO
    {
        /// <summary>
        /// Identifica el Tipo de Adquisición del Individuo
        /// </summary>
        [JsonProperty("tipoAdquisicionId")]
        public decimal TipoAdquisicionId { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [JsonProperty("nombre")]
        [Display(Name = "Nombre")]
        [MaxLength(1000, ErrorMessage = "NombreMaxLength")]
        [Required(ErrorMessage = "NombreRequired")]
        public string Nombre { get; set; } = string.Empty;


        /// <summary>
        /// Descripcion
        /// </summary>
        [JsonProperty("descripcion")]
        [Display(Name = "Descripción")]
        [MaxLength(4000, ErrorMessage = "DescripcionMaxLength")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Eliminado
        /// </summary>
        [JsonProperty("eliminado")]
        [Display(Name = "Eliminado")]
        [MaxLength(1, ErrorMessage = "EliminadoMaxLength")]
        public bool Eliminado { get; set; }

        /// <summary>
        /// Activo
        /// </summary>
        [JsonProperty("activo")]
        [Display(Name = "Activo")]
        [MaxLength(1, ErrorMessage = "ActivoMaxLength")]
        public bool Activo { get; set; }
    }
}

