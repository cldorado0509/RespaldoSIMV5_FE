namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class PuntoControlExpedienteDocumentalDTO
    {
        [JsonProperty("idExpediente")]
        public int idExpediente { get; set; }

        [JsonProperty("idPuntoControl")]
        public int idPuntoControl { get; set; }
    }
}