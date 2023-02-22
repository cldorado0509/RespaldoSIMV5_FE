namespace SIM.Areas.DesarrolloEconomico.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class Producto
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("terceroId")]
        public decimal TerceroId { get; set; }

        [JsonProperty("nombreProducto", Required = Required.Always)]
        [Required]
        public string NombreProducto { get; set; }

        [JsonProperty("descripcionProducto")]
        public string DescripcionProducto { get; set; }

        [JsonProperty("urlImagen")]
        public string UrlImagen { get; set; }

        [JsonProperty("unidadMed")]
        public string UnidadMed { get; set; }

        [JsonProperty("valorUnitario")]
        public decimal ValorUnitario { get; set; }

        [JsonProperty("activo")]
        public bool Activo { get; set; }
    }
}