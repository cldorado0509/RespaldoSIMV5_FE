using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class ProcesoJudicialDTO
    {
        [JsonProperty("procesoId")]
        public int ProcesoId { get; set; }

        [JsonProperty("procesoJuzgadoId")]
        public decimal? ProcesoJuzgadoId { get; set; }

        [JsonProperty("procuraduriasId")]
        public decimal? ProcuraduriasId { get; set; }

        [JsonProperty("contrato")]
        public string Contrato { get; set; }

        [JsonProperty("terceroId")]
        public int? TerceroId { get; set; }

        [JsonProperty("apoderado")]
        public string Apoderado { get; set; } = string.Empty;

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("fechaRadicado")]
        public DateTime? FechaRadicado { get; set; }

        [JsonProperty("cuantia")]
        [StringLength(1)]
        public string Cuantia { get; set; }

        [JsonProperty("hechos")]
        [StringLength(2000)]
        public string Hechos { get; set; }

        [JsonProperty("fechaNotificacion")]
        public DateTime? FechaNotificacion { get; set; }

        [JsonProperty("instanciaId")]
        public decimal? InstanciaId { get; set; }

        [JsonProperty("procesoCodigoId")]
        public decimal? ProcesoCodigoId { get; set; }

        [JsonProperty("fechaIngreso")]
        public DateTime? FechaIngreso { get; set; }

        [JsonProperty("procesoEstadoId")]
        public decimal? ProcesoEstadoId { get; set; }

        [JsonProperty("tipoDemanda")]
        [StringLength(4)]
        public string TipoDemanda { get; set; }

        [JsonProperty("terminado")]
        [StringLength(1)]
        public string Terminado { get; set; }

        [JsonProperty("eliminado")]
        [Required]
        [StringLength(1)]
        public string Eliminado { get; set; } = string.Empty;

        [JsonProperty("radicado21")]
        [StringLength(21)]
        public string Radicado21 { get; set; }

        [JsonProperty("sincronizado")]
        [Required]
        [StringLength(1)]
        public string Sincronizado { get; set; } = string.Empty;

        [JsonProperty("mensajeSincronizacion")]
        [StringLength(200)]
        public string MensajeSincronizacion { get; set; }

        [JsonProperty("fundamentoJuridicoConvocante")]
        [StringLength(4000)]
        public string FundamentoJuridicoConvocante { get; set; }

        [JsonProperty("fundamentoDefensa")]
        [StringLength(4000)]
        public string FundamentoDefensa { get; set; }

        [JsonProperty("recomendacionesAbogado")]
        [StringLength(4000)]
        public string RecomendacionesAbogado { get; set; }

        [JsonProperty("fechaComiteConciliacion")]
        public DateTime? FechaComiteConciliacion { get; set; }

        [JsonProperty("decisionComite")]
        [StringLength(1)]
        public string DecisionComite { get; set; }

        [JsonProperty("fechaAudienciaPrejudicial")]
        public DateTime? FechaAudienciaPrejudicial { get; set; }

        [JsonProperty("hayAcuerdo")]
        [StringLength(1)]
        public string HayAcuerdo { get; set; }

        [JsonProperty("decisionAudiencia")]
        [StringLength(1)]
        public string DecisionAudiencia { get; set; }

        [JsonProperty("demandantes")]
        public List<DemandantesDTO> Demandantes { get; set; } = new List<DemandantesDTO>();

        [JsonProperty("demandados")]
        public List<DemandadosDTO> Demandados { get; set; } = new List<DemandadosDTO>();

        [JsonProperty("resumen")]
        [StringLength(4000)]
        public string Resumen { get; set; } = String.Empty;

        [JsonProperty("asunto")]
        [StringLength(4000)]
        public string Asunto { get; set; } = String.Empty;

    }
}
