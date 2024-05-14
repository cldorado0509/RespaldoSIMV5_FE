using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{
    /// <summary>
    /// Examen Laboratorio DTO
    /// </summary>
    public class ExamenLaboratorioDTO
    {
        /// <summary>
        /// Identifica el Examen de Laboratorio
        /// </summary>
        [JsonProperty("ExamenLaboratorioId")]
        public decimal ExamenLaboratorioId { get; set; }

        /// <summary>
        /// Identifica el Individuo registrado en el CAV
        /// </summary>
        [JsonProperty("RegistroCAVId")]
        public decimal RegistroCAVId { get; set; }

        /// <summary>
        /// Identifica la Historia Clínica a la que pertenece el Exámen de laboratorio 
        /// </summary>
        [JsonProperty("HistoriaId")]
        public decimal? HistoriaId { get; set; }

        /// <summary>
        /// Número de Identificación del individuo
        /// </summary>
        [MaxLength(20, ErrorMessage = "NroPruebaMaxLength")]
        [Required]
        [JsonProperty("NumeroPrueba")]
        public string NumeroPrueba { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de del Exámen
        /// </summary>
        [Required]
        [JsonProperty("Fecha")]
        public DateTime Fecha { get; set; }


        /// <summary>
        /// Observaciones relacionadas con el Exámen de Laboratorio
        /// </summary>
        [MaxLength(2000, ErrorMessage = "ObservacionMaxLength")]
        [JsonProperty("Observacion")]
        public string Observacion { get; set; } = string.Empty;
    }
}
