using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class TramiteDTO
    {
        [JsonProperty("codTramite")]
        public decimal CodTramite { get; set; }

        [JsonProperty("codProceso")]
        public decimal CodProceso { get; set; }

        [JsonProperty("codTarea")]
        public decimal codTarea { get; set; }

        [JsonProperty("codFuncionario")]
        public decimal CodFuncionario { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("cedula")]
        public string Cedula { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("comentarios")]
        public string Comentarios { get; set; }

        [JsonProperty("fechaIni")]
        public DateTime? FechaIni { get; set; }

        [JsonProperty("fechaFin")]
        public DateTime? FechaFin { get; set; }


        [JsonProperty("fechaLimite")]
        public DateTime? FechaLimite { get; set; }

        [JsonProperty("prioridad")]
        public decimal? Prioridad { get; set; }

        [JsonProperty("estado")]
        public decimal? Estado { get; set; }

        [JsonProperty("carpeta")]
        public string Carpeta { get; set; }

        [JsonProperty("clave")]
        public string Clave { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("tiempoAcumulado")]
        public decimal? TiempoAcumulado { get; set; }

        [JsonProperty("diasAcumulados")]
        public decimal? DiasAcumulados { get; set; }

        [JsonProperty("horasAcumuladas")]
        public string HorasAcumuladas { get; set; }

        [JsonProperty("minutosAcumulados")]
        public decimal? MinutosAcumulados { get; set; }

        [JsonProperty("diasAcumuladosHabiles")]
        public decimal? DiasAcumuladosHabiles { get; set; }

        [JsonProperty("horasAcumuladasHabiles")]
        public decimal? HorasAcumuladasHabiles { get; set; }

        [JsonProperty("minutosAcumuladosHabiles")]
        public decimal? MinutosAcumuladosHabiles { get; set; }

        [JsonProperty("numeroVital")]
        public string NumeroVital { get; set; }

        [JsonProperty("numeroVitalPadre")]
        public string NumeroVitalPadre { get; set; }

        [JsonProperty("codTramiteAnterior")]
        public decimal? CodTramiteAnterior { get; set; }

        [JsonProperty("agrupador")]
        public decimal? Agrupador { get; set; }
    }
}