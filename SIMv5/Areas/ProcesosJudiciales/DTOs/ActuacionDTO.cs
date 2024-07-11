using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    /// <summary>
    /// Actuacion DTO
    /// </summary>
    public class ActuacionDTO
    {
        /// <summary>
        /// Identifica la Actuación
        /// </summary>
        [JsonProperty("actuacionId")]
        public int ActuacionId { get; set; }

        /// <summary>
        /// Identifica el proceso judicial
        /// </summary>
        [JsonProperty("procesoJuridicoId")]
        [Required]
        public int ProcesoJuridicoId { get; set; }

        /// <summary>
        /// Identifica el Tipo de actuación
        /// </summary>
        [JsonProperty("tipoActuacionId")]
        public int? TipoActuacionId { get; set; }

        /// <summary>
        /// Tipo de actuacion
        /// </summary>
        [JsonProperty("tipoActuacion")]
        public string TipoActuacion { get; set; } = string.Empty;


        /// <summary>
        /// Fecha de la actuacion del juzgado
        /// </summary>
        [JsonProperty("fechaFactJuzgado")]
        public DateTime? FechaFactJuzgado { get; set; }

        /// <summary>
        /// Otros
        /// </summary>
        [JsonProperty("otros")]
        [StringLength(200)]
        public string Otros { get; set; }

        /// <summary>
        /// Resumen de la actuación
        /// </summary>
        [JsonProperty("actuacion")]
        [StringLength(2000)]
        public string Actuacion { get; set; }

        /// <summary>
        /// Fecha de la actuación
        /// </summary>
        [JsonProperty("fechaActuacion")]
        [Required]
        public DateTime FechaActuacion { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [JsonProperty("observaciones")]
        [StringLength(2000)]
        public string Observaciones { get; set; }

        /// <summary>
        /// Eliminado
        /// </summary>
        [JsonProperty("eliminado")]
        [Required]
        [StringLength(1)]
        public string Eliminado { get; set; } = "0";

        /// <summary>
        /// Ruta
        /// </summary>
        [JsonProperty("ruta")]
        [StringLength(255)]
        public string Ruta { get; set; }

        /// <summary>
        /// Identfica la Notificación en Litigio Virtual
        /// </summary>
        [JsonProperty("noficacionJudicialId")]
        public int? NoficacionJudicialId { get; set; }

        /// <summary>
        /// Identifica el Juzgado
        /// </summary>
        [JsonProperty("codigoJuzgado")]
        [StringLength(20)]
        public string CodigoJuzgado { get; set; }

        /// <summary>
        /// Fecha de registro de la actuación
        /// </summary>
        [JsonProperty("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("estaSincronizado")]
        [Required]
        [StringLength(1)]
        public string EstaSincronizado { get; set; } = "0";

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("mensajeSincronizacion")]
        [StringLength(200)]
        public string MensajeSincronizacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rutaSIM")]
        [StringLength(255)]
        public string RutaSIM { get; set; }
    }
}