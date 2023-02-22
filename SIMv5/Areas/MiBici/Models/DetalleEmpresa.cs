namespace SIM.Areas.MiBici.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class DetalleEmpresa
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("idTercero")]
        public decimal IdTercero { get; set; }

        [JsonProperty("idInstalacion")]
        public decimal IdInstalacion { get; set; }

        [JsonProperty("idCategoria")]
        public decimal IdCategoria { get; set; }

        [JsonProperty("sitioWeb", Required = Required.Always)]
        [Required]
        public string SitioWeb { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("instagram")]
        public string Instagram { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("eMail")]
        public string EMail { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("whatsApp")]
        public string WhatsApp { get; set; }

        [JsonProperty("logo")]
        public byte[] Logo { get; set; }

        [JsonProperty("foto")]
        public byte[] Foto { get; set; }
    }
}