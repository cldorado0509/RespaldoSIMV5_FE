namespace SIM.Areas.MiBici.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class ProductoFotos
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("terceroId")]
        public decimal TerceroId { get; set; }

        [JsonProperty("instalacionId")]
        public decimal InstalacionId { get; set; }

        [JsonProperty("nombreProducto", Required = Required.Always)]
        [Required]
        public string NombreProducto { get; set; }

        [JsonProperty("descripcionProducto")]
        public string DescripcionProducto { get; set; }

        [JsonProperty("fotos")]
        public List<byte[]> Fotos { get; set; }

       

    }
}