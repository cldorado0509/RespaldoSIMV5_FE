namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class EstadoPuntoControlDTO
    {
        /// <summary>
        /// Identifica el Estado del Punto de Control
        /// </summary>
        [JsonProperty("idEstadoPuntoControl")]
        public int IdEstadoPuntoControl { get; set; }


        /// <summary>
        /// Identifica el punto de control
        /// </summary>
        [JsonProperty("puntoControlId")]
        [Required]
        public int PuntoControlId { get; set; }

        /// <summary>
        /// Identifica el Tipo de Estado del Punto de Control
        /// </summary>
        [JsonProperty("tipoEstadoPuntoControlId")]
        [Required]
        public int TipoEstadoPuntoControlId { get; set; }

        /// <summary>
        /// Tipo de Estado del Punto de Control
        /// </summary>
        [JsonProperty("tipoEstadoPuntoControl")]
        public string TipoEstadoPuntoControl { get; set; }

        /// <summary>
        /// Identifica el Funcionario
        /// </summary>
        [JsonProperty("funcionarioId")]
        [Required]
        public int FuncionarioId { get; set; }

        /// <summary>
        /// Fecha de inicio del estado del punto de control
        /// </summary>
        [JsonProperty("fechaEstado")]
        [Required]
        public DateTime FechaEstado { get; set; }


        /// <summary>
        /// Observaciones relacionadas con el estado del punto de control
        /// </summary>
        [JsonProperty("observacion")]
        public string Observacion { get; set; }



        /// <summary>
        /// Identifica el Tercero
        /// </summary>
        [JsonProperty("terceroId")]
        public int? TerceroId { get; set; }
    }
}