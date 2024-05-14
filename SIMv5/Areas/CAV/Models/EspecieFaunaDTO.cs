
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{

    /// <summary>
    /// Especie Fauna DTO
    /// </summary>
    public class EspecieFaunaDTO
    {
        /// <summary>
        /// Identifica la Especie
        /// </summary>
        [JsonProperty("especieFaunaId")]
        public decimal EspecieFaunaId { get; set; }

        /// <summary>
        /// Identifica la familia
        /// </summary>
        [JsonProperty("familiaFaunaId")]
        public decimal FamiliaFaunaId { get; set; }


        /// <summary>
        /// Nombre Común
        /// </summary>
        [Display(Name = "Nombre Común")]
        [MaxLength(500, ErrorMessage = "NombreComunMaxLength")]
        [Required(ErrorMessage = "NombreComunRequired")]
        [JsonProperty("nombreComun")]
        public string NombreComun { get; set; } = string.Empty;

        /// <summary>
        /// Nombre Científico
        /// </summary>
        [Display(Name = "Nombre Científico")]
        [MaxLength(500, ErrorMessage = "NombreCientificoMaxLength")]
        [JsonProperty("nombreCientifico")]
        public string NombreCientifico { get; set; }

        /// <summary>
        /// Nombre Científico
        /// </summary>
        [Display(Name = "Valor Comercial")]
        [JsonProperty("valorComercial")]
        public decimal? ValorComercial { get; set; }

        /// <summary>
        /// Nombre Científico
        /// </summary>
        [Display(Name = "Valor Ecológico")]
        [JsonProperty("valorEcologico")]
        public decimal? ValorEcologico { get; set; }

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
        /// Identifica la Categoría
        /// </summary>
        [Required]
        [JsonProperty("categoriaUICNId")]
        public decimal CategoriaUICNId { get; set; }

        /// <summary>
        /// Es Endémica
        /// </summary>
        [Display(Name = "Es Endémica")]
        [MaxLength(1, ErrorMessage = "EsEndemicaMaxLength")]
        [JsonProperty("esEndemica")]
        public string EsEndemica { get; set; }

        /// <summary>
        /// Código Contable
        /// </summary>
        [Display(Name = "Código Contable")]
        [MaxLength(8, ErrorMessage = "CodigoContableMaxLength")]
        [JsonProperty("codigoContable")]
        public string CodigoContable { get; set; } = "83250635";

        /// <summary>
        /// Código Contable
        /// </summary>
        [Display(Name = "Código Auxiliar")]
        [MaxLength(2, ErrorMessage = "CodigoAuxiliarMaxLength")]
        [JsonProperty("codigoSubAuxiliar")]
        public string CodigoSubAuxiliar { get; set; }

        /// <summary>
        /// Código Contable
        /// </summary>
        [Display(Name = "Código Referencia")]
        [MaxLength(5, ErrorMessage = "CodigoReferenciaMaxLength")]
        [JsonProperty("codigoReferencia")]
        public string CodigoReferencia { get; set; }
    }

}
