namespace SIM.Areas.AdministracionDocumental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class SubSerieDocumental
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("serieId")]
        public decimal SerieId { get; set; }

        [JsonProperty("nombre", Required = Required.Always)]
        [Required]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("habilitado")]
        public bool Habilitado { get; set; }

        public override string ToString()
        {
            return $"{this.Nombre}";
        }
    }
}