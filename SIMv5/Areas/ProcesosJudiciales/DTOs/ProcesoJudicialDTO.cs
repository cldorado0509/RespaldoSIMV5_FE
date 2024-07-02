using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    /// <summary>
    /// ProcesoJudicialDTO
    /// </summary>
    public class ProcesoJudicialDTO
    {
        /// <summary>
        /// Identifica el proceso judicial
        /// </summary>
        [JsonProperty("procesoId")]
        public int ProcesoId { get; set; }

        /// <summary>
        /// Identifica el juzgado
        /// </summary>
        [JsonProperty("procesoJuzgadoId")]
        public decimal? ProcesoJuzgadoId { get; set; }

        /// <summary>
        /// Identifica la procuraduría
        /// </summary>
        [JsonProperty("procuraduriasId")]
        public decimal? ProcuraduriasId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("contrato")]
        public string Contrato { get; set; }

        /// <summary>
        /// Identifica el apoderado
        /// </summary>
        [JsonProperty("terceroId")]
        public int? TerceroId { get; set; }

        /// <summary>
        /// Nombre del apoderado
        /// </summary>
        [JsonProperty("apoderado")]
        public string Apoderado { get; set; } = string.Empty;

        /// <summary>
        /// Radicado
        /// </summary>
        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        /// <summary>
        /// Fecha del radicado
        /// </summary>
        [JsonProperty("fechaRadicado")]
        public DateTime? FechaRadicado { get; set; }

        /// <summary>
        /// Establece cuantía
        /// </summary>
        [JsonProperty("cuantia")]
        [StringLength(1)]
        public string Cuantia { get; set; }

        /// <summary>
        /// Resumén de los hechos
        /// </summary>
        [JsonProperty("hechos")]
        [StringLength(2000)]
        public string Hechos { get; set; }

        /// <summary>
        /// Fecha de Notificación
        /// </summary>
        [JsonProperty("fechaNotificacion")]
        public DateTime? FechaNotificacion { get; set; }

        /// <summary>
        /// Identifica la instancia
        /// </summary>
        [JsonProperty("instanciaId")]
        public decimal? InstanciaId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("procesoCodigoId")]
        public decimal? ProcesoCodigoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fechaIngreso")]
        public DateTime? FechaIngreso { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("procesoEstadoId")]
        public decimal? ProcesoEstadoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("tipoDemanda")]
        [StringLength(4)]
        public string TipoDemanda { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("terminado")]
        [StringLength(1)]
        public string Terminado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("eliminado")]
        [Required]
        [StringLength(1)]
        public string Eliminado { get; set; } = string.Empty;

        /// <summary>
        /// Radicado único
        /// </summary>
        [JsonProperty("radicado21")]
        [StringLength(21)]
        public string Radicado21 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("sincronizado")]
        [Required]
        [StringLength(1)]
        public string Sincronizado { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("mensajeSincronizacion")]
        [StringLength(200)]
        public string MensajeSincronizacion { get; set; }

        /// <summary>
        /// Fundamentos jurídicos del convocante
        /// </summary>
        [JsonProperty("fundamentoJuridicoConvocante")]
        [StringLength(4000)]
        public string FundamentoJuridicoConvocante { get; set; }

        /// <summary>
        /// Fundamentos de la defensa
        /// </summary>
        [JsonProperty("fundamentoDefensa")]
        [StringLength(4000)]
        public string FundamentoDefensa { get; set; }

        /// <summary>
        /// Recomendaciones del abogado
        /// </summary>
        [JsonProperty("recomendacionesAbogado")]
        [StringLength(4000)]
        public string RecomendacionesAbogado { get; set; }

        /// <summary>
        /// Fecha establecida para la realización del comité de conciliación
        /// </summary>
        [JsonProperty("fechaComiteConciliacion")]
        public DateTime? FechaComiteConciliacion { get; set; }

        /// <summary>
        /// Desición del comité
        /// </summary>
        [JsonProperty("decisionComite")]
        [StringLength(1)]
        public string DecisionComite { get; set; }


        /// <summary>
        /// Fecha de la audiencia definida en la etapa extrajudial
        /// </summary>
        [JsonProperty("fechaAudienciaPrejudicial")]
        public DateTime? FechaAudienciaPrejudicial { get; set; }

        /// <summary>
        /// Establece si hay acuerdo entre las partes
        /// </summary>
        [JsonProperty("hayAcuerdo")]
        [StringLength(1)]
        public string HayAcuerdo { get; set; }

        /// <summary>
        /// Decisión de la audiencia
        /// </summary>
        [JsonProperty("decisionAudiencia")]
        [StringLength(1)]
        public string DecisionAudiencia { get; set; }

        /// <summary>
        /// Lista de demandantes
        /// </summary>
        [JsonProperty("demandantes")]
        public List<DemandantesDTO> Demandantes { get; set; } = new List<DemandantesDTO>();

        /// <summary>
        /// Lista de demandados
        /// </summary>
        [JsonProperty("demandados")]
        public List<DemandadosDTO> Demandados { get; set; } = new List<DemandadosDTO>();

        /// <summary>
        /// Resumen
        /// </summary>
        [JsonProperty("resumen")]
        [StringLength(4000)]
        public string Resumen { get; set; } = String.Empty;

        /// <summary>
        /// Asunto
        /// </summary>
        [JsonProperty("asunto")]
        [StringLength(4000)]
        public string Asunto { get; set; } = String.Empty;

        /// <summary>
        /// Riesgo procesal
        /// </summary>
        [JsonProperty("riesgoProcesal")]
        [StringLength(4000)]
        public string RiesgoProcesal { get; set; }

        /// <summary>
        /// Valor cuantía
        /// </summary>
        [JsonProperty("valorCuantia")]
        public long? ValorCuantia { get; set; }

        /// <summary>
        /// Política institucional
        /// </summary>
        [JsonProperty("politicaInstitucional")]
        [StringLength(4000)]
        public string PoliticaInstitucional { get; set; }

        /// <summary>
        /// Llama en garantía
        /// </summary>
        [JsonProperty("llamaEnGarantia")]
        [StringLength(1)]
        public string LlamaEnGarantia { get; set; }

        /// <summary>
        /// Caducidad
        /// </summary>
        [JsonProperty("caducidad")]
        [StringLength(1)]
        public string Caducidad { get; set; } = "0";


        /// <summary>
        /// Pretenciones
        /// </summary>
        [JsonProperty("pretenciones")]
        [StringLength(4000)]
        public string Pretenciones { get; set; }

    }
}
