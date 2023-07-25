using Newtonsoft.Json;
using System;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TramiteDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("codTramite")]
        public decimal CodTramite { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("codProceso")]
        public decimal CodProceso { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("codTarea")]
        public decimal codTarea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("codFuncionario")]
        public decimal CodFuncionario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("cliente")]
        public string Cliente { get; set; } = string.Empty;

        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("cedula")]
        public string Cedula { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("direccion")]
        public string Direccion { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("telefono")]
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("mail")]
        public string Mail { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("comentarios")]
        public string Comentarios { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fechaIni")]
        public DateTime? FechaIni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fechaFin")]
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fechaLimite")]
        public DateTime? FechaLimite { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("prioridad")]
        public decimal? Prioridad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("estado")]
        public decimal? Estado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("carpeta")]
        public string Carpeta { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("clave")]
        public string Clave { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("tiempoAcumulado")]
        public decimal? TiempoAcumulado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("diasAcumulados")]
        public decimal? DiasAcumulados { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("horasAcumuladas")]
        public string HorasAcumuladas { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("minutosAcumulados")]
        public decimal? MinutosAcumulados { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("diasAcumuladosHabiles")]
        public decimal? DiasAcumuladosHabiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("horasAcumuladasHabiles")]
        public decimal? HorasAcumuladasHabiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("minutosAcumuladosHabiles")]
        public decimal? MinutosAcumuladosHabiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("numeroVital")]
        public string NumeroVital { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("numeroVitalPadre")]
        public string NumeroVitalPadre { get; set; } = string.Empty;

        [JsonProperty("radicadoVITAL")]
        public string RadicadoVITAL { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("codTramiteAnterior")]
        public decimal? CodTramiteAnterior { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("agrupador")]
        public decimal? Agrupador { get; set; }

        /// <summary>
        /// Causa de la no Atencion de Solicitud de VITAL
        /// </summary>
        [JsonProperty("codCausaNoAtencion")]
        public decimal CodCausaNoAtencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("codFuncionarioSIM")]
        public decimal CodFuncionarioSIM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("idSolicitudVITAL")]
        public int IdSolicitudVITAL { get; set; }

    }
}