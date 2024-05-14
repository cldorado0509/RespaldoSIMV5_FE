using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{

    /// <summary>
    /// Municipio DTO
    /// </summary>
    public class MunicipioDTO
    {
        /// <summary>
        /// Identifica el Municipio
        /// </summary>
        [JsonProperty("municipioId")]
        public decimal MunicipioId { get; set; }

        /// <summary>
        /// Identifica el Departamento
        /// </summary>
        [JsonProperty("departamentoId")]
        public decimal DepartamentoId { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        [Display(Name = "Código")]
        [MaxLength(20, ErrorMessage = "CodigoMaxLength")]
        [Required(ErrorMessage = "CodigoRequired")]
        [JsonProperty("codigo")]
        public string Codigo { get; set; } = string.Empty;


        /// <summary>
        /// Nombre
        /// </summary>
        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "NombreMaxLength")]
        [Required(ErrorMessage = "NombreRequired")]
        [JsonProperty("nombre")]
        public string Nombre { get; set; } = string.Empty;


        /// <summary>
        /// Nombre
        /// </summary>
        [Display(Name = "Amva")]
        [MaxLength(1, ErrorMessage = "AmvaMaxLength")]
        [JsonProperty("amva")]
        public string Amva { get; set; } = string.Empty;

    }
}
