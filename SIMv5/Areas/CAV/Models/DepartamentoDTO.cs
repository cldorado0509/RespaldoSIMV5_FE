using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{
    /// <summary>
    /// Departamento DTO
    /// </summary>
    public class DepartamentoDTO
    {
        /// <summary>
        /// Identifica el Departamento
        /// </summary>
        [JsonProperty("departamentoId")]
        public decimal DepartamentoId { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        [MaxLength(20, ErrorMessage = "CodigoMaxLength")]
        [Required(ErrorMessage = "CodigoRequired")]
        [JsonProperty("codigo")]
        public string Codigo { get; set; } = string.Empty;


        /// <summary>
        /// Nombre
        /// </summary>
        [JsonProperty("nombre")]
        [MaxLength(100, ErrorMessage = "NombreMaxLength")]
        [Required(ErrorMessage = "NombreRequired")]
        public string Nombre { get; set; } = string.Empty;

    }
}