namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class TramiteSeguimiento
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("codigoSolicitud", Required = Required.Always)]
        [Required]
        public decimal CodigoSolicitud { get; set; }

        [JsonProperty("codigoActoAdministrativo")]
        public int CodigoActoAdministrativo { get; set; }


        [JsonProperty("codigoTramite", Required = Required.Always)]
        [Required]
        public int CodigoTramite { get; set; }

        [JsonProperty("codigoDocumento", Required = Required.Always)]
        [Required]
        public int CodigoDocumento { get; set; }


        [JsonProperty("cm")]
        public string CM { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("anio")]
        public int Anio { get; set; }

        [JsonProperty("radicadoSalida")]
        public string radicadoSalida { get; set; }

        [JsonProperty("proyecto")]
        public string Proyecto { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }

        [JsonProperty("tecnico")]
        public string Tecnico { get; set; }

        [JsonProperty("apoyo")]
        public string Apoyo { get; set; }

        [JsonProperty("solicitante")]
        public string Solicitante { get; set; }
    }
}