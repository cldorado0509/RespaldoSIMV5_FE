
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class AbogadoExpedienteDTO
    {
        /// <summary>
        /// Identifica el abogado asociado con el Expediente
        /// </summary>
        [JsonProperty("idAbogadoExpediente")]
        public int IdAbogadoExpediente { get; set; }


        /// <summary>
        /// Identifica el Expediente Ambiental
        /// </summary>
        [JsonProperty("expedienteAmbientalId")]
        public int ExpedienteAmbientalId { get; set; }


        /// <summary>
        /// Identifica el Abogado
        /// </summary>
        [JsonProperty("funcionarioId")]
        public int FuncionarioId { get; set; }

        /// <summary>
        /// Fecha de Asignación del expediente ambiental al abogado
        /// </summary>
        [JsonProperty("fechaAsignacion")]
        [Required]
        public DateTime FechaAsignacion { get; set; }

        /// <summary>
        /// Fecha de finalización de la intervención del abogado sobre el expediente ambiental
        /// </summary>
        [JsonProperty("fechaFin")]
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Observaciones relacionadas con la actuación del abogado sobre el expediente
        /// </summary>
        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("abogado")]
        public string Abogado { get; set; }

    }
}