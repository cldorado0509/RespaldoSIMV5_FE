using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class SolicitudVITALDTO
    {
        /// <summary>
        /// Clave Primaria
        /// </summary>
        [JsonProperty("id")]
        public decimal Id { get; set; }

        /// <summary>
        /// Identifica el Tipo de Trámite VITAL
        /// </summary>
        [JsonProperty("tipoTramite")]
        [Required]
        public string TipoTramite { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del Trámite en la plataforma VITAL
        /// </summary>
        [Required]
        [JsonProperty("identificador")]
        public string Identificador { get; set; } = string.Empty;


        /// <summary>
        /// Identifica el Trámite en el SIM
        /// </summary>
        [JsonProperty("codigoTramiteSIM")]
        public decimal? CodigoTramiteSIM { get; set; }

        /// <summary>
        /// Fecha de ingreso de la solicitud VITAL
        /// </summary>
        [Required]
        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Establece si ya la solicitud que llegá de VITAL fue Atendida desde el SIM
        /// </summary>
        [Required]
        [JsonProperty("solicitudAtendida")]
        public string SolicitudAtendida { get; set; } = string.Empty;

        /// <summary>
        /// Identificación del Radicado del Documento
        /// </summary>
        [JsonProperty("radicacionId")]
        public string RadicacionId { get; set; }

        /// <summary>
        /// Número SILPA
        /// </summary>
        [Required]
        [JsonProperty("numeroSILPA")]
        public string NumeroSILPA { get; set; } = string.Empty;

        /// <summary>
        /// Número del Acto Administrativo Asociado
        /// </summary>
        [JsonProperty("actoAdministrativo")]
        public string ActoAdministrativo { get; set; }

        /// <summary>
        /// Identificación del Formulario VITAL
        /// </summary>
        [Required]
        [JsonProperty("formularioId")]
        public string FormularioId { get; set; } = string.Empty;

        /// <summary>
        /// Ruta del Documento
        /// </summary>
        [JsonProperty("rutaDocumento")]
        public string RutaDocumento { get; set; }

        /// <summary>
        /// Identificación de la Autoridad Ambiental
        /// </summary>
        [Required]
        [JsonProperty("autoridadAmbientalID")]
        public string AutoridadAmbientalID { get; set; } = string.Empty;

        /// <summary>
        /// Número de VITAL
        /// </summary>
        [Required]
        [JsonProperty("numeroVITAL")]
        public string NumeroVITAL { get; set; } = string.Empty;

        /// <summary>
        /// Número de VITAL Asociado
        /// </summary>
        [JsonProperty("numeroVITALAsociado")]
        public string NumeroVITALAsociado { get; set; }
    }
}