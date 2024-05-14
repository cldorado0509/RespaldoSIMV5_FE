using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{

    /// <summary>
    /// Historia Clínica DTO
    /// </summary>
    public class HistoriaDTO
    {

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("historiaId")]
        public decimal HistoriaId { get; set; }

        /// <summary>
        /// Identifica el Individuo registrado en el CAV
        /// </summary>
        [JsonProperty("registroCAVId")]
        public decimal RegistroCAVId { get; set; }


        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "Observaciones")]
        [MaxLength(4000, ErrorMessage = "ObservacionMaxLength")]
        [JsonProperty("observacion")]
        public string Observacion { get; set; }


        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "Funcionario Responsable")]
        [MaxLength(200, ErrorMessage = "FuncionarioResponsableMaxLength")]
        [JsonProperty("funcionarioResponsable")]
        public string FuncionarioResponsable { get; set; } = string.Empty;

        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "Estudiante")]
        [MaxLength(200, ErrorMessage = "EstudianteMaxLength")]
        [JsonProperty("estudiante")]
        public string Estudiante { get; set; } = string.Empty;


    }
}
