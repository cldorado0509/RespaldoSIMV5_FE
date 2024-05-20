using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class TipoEdadDTO
    {
        /// <summary>
        /// Identifica el Tipo de Edad del Individuo
        /// </summary>
        [JsonProperty("tipoEdadId")]
        public decimal TipoEdadId { get; set; }

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
        public string Eliminado { get; set; }

        /// <summary>
        /// Activo
        /// </summary>
        [JsonProperty("activo")]
        [Display(Name = "Activo")]
        [MaxLength(1, ErrorMessage = "ActivoMaxLength")]
        public string Activo { get; set; }
    }
}
