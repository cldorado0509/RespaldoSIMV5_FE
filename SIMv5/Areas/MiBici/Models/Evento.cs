namespace SIM.Areas.MiBici.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Evento
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("terceroId")]
        public decimal TerceroId { get; set; }

        [JsonProperty("instalacionId")]
        public decimal InstalacionId { get; set; }

        [JsonProperty("fechaEvento", Required = Required.Always)]
        [Required]
        public DateTime FechaEvento { get; set; }

        [JsonProperty("descripcionEvento", Required = Required.Always)]
        [Required]
        public string DescripcionEvento { get; set; }

        [JsonProperty("lugar")]
        public string Lugar { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("valor", Required = Required.Always)]
        [Required]
        public long Valor { get; set; }

        [JsonProperty("informacionContacto")]
        public string InformacionContacto { get; set; }
    }
}