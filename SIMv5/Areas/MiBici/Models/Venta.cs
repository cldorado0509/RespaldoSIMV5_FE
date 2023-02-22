namespace SIM.Areas.MiBici.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Venta
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("terceroId")]
        public decimal TerceroId { get; set; }

        [JsonProperty("instalacionId")]
        public decimal InstalacionId { get; set; }

        [JsonProperty("fechaVenta", Required = Required.Always)]
        [Required]
        public DateTime FechaVenta { get; set; }

        [JsonProperty("identificacionCliente", Required = Required.Always)]
        [Required]
        public string IdentificacionCliente { get; set; }

        [JsonProperty("marca")]
        public string Marca { get; set; }

        [JsonProperty("referencia", Required = Required.Always)]
        [Required]
        public string Referencia { get; set; }

        [JsonProperty("serial")]
        public string Serial { get; set; }
    }
}