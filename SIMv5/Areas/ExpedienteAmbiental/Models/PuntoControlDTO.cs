namespace SIM.Areas.ExpedienteAmbiental.Models.DTO
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class PuntoControlDTO
    {
        /// <summary>
        /// Identifica el Punto de Control
        /// </summary>
        [JsonProperty("idPuntoControl")]
        public int IdPuntoControl { get; set; }

        /// <summary>
        /// Identifica el Expediente ambiental
        /// </summary>
        [JsonProperty("expedienteAmbientalId")]
        public int ExpedienteAmbientalId { get; set; }

        /// <summary>
        /// Identifica el Expediente Documental asociado al Expediente Ambiental
        /// </summary>
        [JsonProperty("expedienteDocumentalId")]
        public int? ExpedienteDocumentalId { get; set; }

        [JsonProperty("expedienteDocumentalLabel")]
        public string ExpedienteDocumentalLabel { get; set; }


        [JsonProperty("expedienteDocumentalCodigo")]
        public string ExpedienteDocumentalCodigo { get; set; }

        /// <summary>
        /// Establece el tipo de solicitud ambiental asociada al punto de control
        /// </summary>
        [Required]
        [JsonProperty("tipoSolicitudAmbientalId")]
        public int TipoSolicitudAmbientalId { get; set; }

        /// <summary>
        /// Nombre del Tipo de Solicitud Ambiental
        /// </summary>
        [JsonProperty("tipoSolicitudAmbiental")]
        public string TipoSolicitudAmbiental { get; set; }

        /// <summary>
        /// Identifica el Expediente ambiental en la estructura anterior del SIM v4
        /// </summary>
        [JsonProperty("codigoSolicitudId")]
        public int? CodigoSolicitudId { get; set; }

        /// <summary>
        /// Nombre del punto de control
        /// </summary>
        [MaxLength(254)]
        [Required]
        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// nombre del tipo de Componenteambiental
        /// </summary>
        [MaxLength(60)]
        [Required]
        [JsonProperty("conexo")]
        public string Conexo { get; set; }

        /// <summary>
        /// Observación relacionada con el punto de control
        /// </summary>
        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("fechaOrigen")]
        public DateTime? FechaOrigen { get; set; }

        [JsonProperty("fechaInicio")]
        public DateTime? FechaInicio { get; set; }

        [JsonProperty("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; }

    }
}