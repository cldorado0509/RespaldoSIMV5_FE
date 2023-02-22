namespace SIM.Areas.ExpedienteAmbiental.Models 
{

    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class AnotacionPuntoControlDTO
    {
        /// <summary>
        /// Identifica la Anotación asociada al Punto de Control
        /// </summary>
        [JsonProperty("idAnotacionPuntoControl")]
        public int IdAnotacionPuntoControl { get; set; }


        /// <summary>
        /// Identifica el punto de control
        /// </summary>
        [JsonProperty("puntoControlId")]
        [Required]
        public int PuntoControlId { get; set; }

        /// <summary>
        /// Identifica el Funcionario que realiza la anotación
        /// </summary>
        [JsonProperty("funcionarioId")]
        public int FuncionarioId { get; set; }

        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }

        /// <summary>
        /// Fecha de la anotación
        /// </summary>
        [Required]
        [JsonProperty("fechaRegistro")]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [JsonProperty("anotacion")]
        public string Anotacion { get; set; }
    }
}