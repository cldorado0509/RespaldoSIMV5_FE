using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AMVA.CAV.Core.DTOs
{
    /// <summary>
    /// Autoridad 
    /// </summary>
    public class AutoridadDTO
    {
        /// <summary>
        /// Identifica la autoridad o entidad responsable de la Fauna
        /// </summary>
        [JsonProperty("autoridadId")]
        public decimal AutoridadId { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "NombreMaxLength")]
        [Required(ErrorMessage = "NombreRequired")]
        [JsonProperty("nombre")]
        public string Nombre { get; set; } = string.Empty;


        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "Descripción")]
        [MaxLength(2000, ErrorMessage = "DescripcionMaxLength")]
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
