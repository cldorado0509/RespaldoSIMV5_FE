namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class EstadoSolicitud
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }


    }
}