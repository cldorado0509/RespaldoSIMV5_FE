namespace SIM.Areas.GestionRiesgo.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class TramiteDTO
    {
        [JsonProperty("codTramite")]
        public decimal CODTRAMITE { get; set; }

        [Required]
        [JsonProperty("codproceso")]
        public decimal CODPROCESO { get; set; }

        [JsonProperty("cliente")]
        [StringLength(200)]
        public string CLIENTE { get; set; }

        [JsonProperty("cedula")]
        [StringLength(100)]
        public string CEDULA { get; set; }

        [JsonProperty("direccion")]
        [StringLength(510)]
        public string DIRECCION { get; set; }

        [JsonProperty("telefono")]
        [StringLength(100)]
        public string TELEFONO { get; set; }

        [JsonProperty("mail")]
        [StringLength(200)]
        public string MAIL { get; set; }

        [JsonProperty("comentarios")]
        [StringLength(2000)]
        public string COMENTARIOS { get; set; }

        [JsonProperty("fechaIni")]
        public DateTime? FECHAINI { get; set; }

        [JsonProperty("fechaFin")]
        public DateTime? FECHAFIN { get; set; }

        [JsonProperty("fechaLimite")]
        public DateTime? FECHALIMITE { get; set; }

        [JsonProperty("prioridad")]
        public decimal? PRIORIDAD { get; set; }

        [JsonProperty("estado")]
        public decimal? ESTADO { get; set; }

        [JsonProperty("carpeta")]
        [StringLength(100)]
        public string CARPETA { get; set; }

        [JsonProperty("clave")]
        [StringLength(200)]
        public string CLAVE { get; set; }

        [JsonProperty("mensaje")]
        [StringLength(500)]
        public string MENSAJE { get; set; }

        [JsonProperty("tiempoAcumulado")]
        public decimal? TIEMPO_ACUMULADO { get; set; }

        [JsonProperty("diasAcumulados")]
        public decimal? DIAS_ACUMULADOS { get; set; }

        [JsonProperty("horasAcumuladas")]
        [StringLength(4000)]
        public string HORAS_ACUMULADAS { get; set; }

        [JsonProperty("minutosAcumulados")]
        public decimal? MINUTOS_ACUMULADOS { get; set; }

        [JsonProperty("diasAcumuladosHabiles")]
        public decimal? DIAS_ACUMULADOS_HABILES { get; set; }

        [JsonProperty("horasAcumuladasHabiles")]
        public decimal? HORAS_ACUMULADAS_HABILES { get; set; }

        [JsonProperty("minitosAcumuladosHabiles")]
        public decimal? MINUTOS_ACUMULADOS_HABILES { get; set; }

        [JsonProperty("numeroVital")]
        [StringLength(30)]
        public string NUMERO_VITAL { get; set; }

        [JsonProperty("codTramiteAnterior")]
        public decimal? CODTRAMITE_ANTERIOR { get; set; }

        [JsonProperty("agrupador")]
        public int? AGRUPADOR { get; set; }
    }
}