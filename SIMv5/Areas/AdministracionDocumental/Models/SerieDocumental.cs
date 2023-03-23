namespace SIM.Areas.AdministracionDocumental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class SerieDocumental
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("nombre", Required = Required.Always)]
        [Required]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("habilitado")]
        public bool Habilitado { get; set; }

        [JsonProperty("radicado")]
        public bool Radicado { get; set; }

        [JsonProperty("version")]
        public decimal Version { get; set; }

        [JsonProperty("codInterno")]
        public string CodInterno { get; set; }

        public override string ToString()
        {
            return $"{this.Nombre}";
        }
    }
}