using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AMVA.CAV.Core.DTOs
{
    /// <summary>
    /// Causa de Ingreso DTO
    /// </summary>
    public class CausaIngresoCAVDTO
    {
        /// <summary>
        /// Identifica la Causa de Ingreso de un Individuo al CAV
        /// </summary>
        [JsonProperty("causaIngresoId")]
        public decimal CausaIngresoId { get; set; }

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
        public string Eliminado { get; set; } = "0";

        /// <summary>
        /// Activo
        /// </summary>
        [Display(Name = "Activo")]
        [MaxLength(1, ErrorMessage = "ActivoMaxLength")]
        [JsonProperty("activo")]
        public string Activo { get; set; } = "1";
    }

}
