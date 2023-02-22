namespace SIM.Areas.DesarrolloEconomico.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class Empresa
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [Required]
        [JsonProperty("idTercero")]
        public decimal IdTercero { get; set; }

        [JsonProperty("razonSocial", Required = Required.Always)]
        [Required]
        public string RazonSocial { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("email")]
        public string EMail { get; set; }

        [JsonProperty("nit")]
        public string Nit { get; set; }

        [JsonProperty("municipio")]
        public string Municipio { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("instagram")]
        public string Instagram { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("web")]
        public string Web { get; set; }

        [JsonProperty("foto")]
        public string Foto { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("telefono1")]
        public string Telefono1 { get; set; }

        [JsonProperty("telefono2")]
        public string Telefono2 { get; set; }

        [JsonProperty("activo")]
        public bool Activo { get; set; }

        public override string ToString()
        {
            return $"{this.RazonSocial}";
        }
    }
}