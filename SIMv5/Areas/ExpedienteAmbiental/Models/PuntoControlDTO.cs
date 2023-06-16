namespace SIM.Areas.ExpedienteAmbiental.Models.DTO
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
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
        /// Identifica la Unidad Documental
        /// </summary>
        [JsonProperty("unidadDocumentalId")]
        public int UnidadDocumentalId { get; set; }


        /// <summary>
        /// Identifica el Funcionario que crea el Expediente
        /// </summary>
        [JsonProperty("funcionarioId")]
        public int FuncionarioId { get; set; }


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

        public List<Indice> Indices { get; set; }

        [JsonProperty("estadoPuntoControl")]
        public string EstadoPuntoControl { get; set; } = string.Empty;

        [JsonProperty("fechaEstadoPuntoControl")]
        public DateTime? FechaEstadoPuntoControl { get; set; }

        public List<IndiceSerieDocumentalDTO> IndicesSerieDocumentalDTO { get; set; }

    }
    public class Indice
    {
        public int CODINDICE { get; set; }
        public string INDICE { get; set; }
        public byte TIPO { get; set; }
        public long LONGITUD { get; set; }
        public int OBLIGA { get; set; }
        public string VALORDEFECTO { get; set; }
        public string VALOR { get; set; }
        public Nullable<int> ID_LISTA { get; set; }
        public Nullable<int> TIPO_LISTA { get; set; }
        public string CAMPO_NOMBRE { get; set; }
        public string MAXIMO { get; set; }
        public string MINIMO { get; set; }
    }
}