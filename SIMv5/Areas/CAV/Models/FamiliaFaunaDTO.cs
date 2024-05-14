using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{
    /// <summary>
    /// DTO Familia Fauna
    /// </summary>
    public class FamiliaFaunaDTO
    {
        /// <summary>
        /// Identifica la Familia de Fauna
        /// </summary>
        [JsonProperty("familiaFaunaId")]
        public decimal FamiliaFaunaId { get; set; }


        /// <summary>
        /// Identifica la Clasificación
        /// </summary>
        [JsonProperty("clasificacionFaunaId")]
        public decimal ClasificacionFaunaId { get; set; }


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

        /// <summary>
        /// Es Protegida
        /// </summary>
        [Display(Name = "Es Protegida")]
        [JsonProperty("esProtegida")]
        public string EsProtegida { get; set; } = string.Empty;

    }
}
