namespace SIM.Areas.AdministracionDocumental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class UnidadDocumental
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("subSerieId")]
        public decimal SubSerieId { get; set; }

        [JsonProperty("nombre", Required = Required.Always)]
        [Required]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [JsonProperty("tiempoGestion")]
        public decimal TiempoGestion { get; set; }

        [Required]
        [JsonProperty("tiempoCentral")]
        public decimal TiempoCentral { get; set; }

        [Required]
        [JsonProperty("tiempoHistorico")]
        public decimal TiempoHistorico { get; set; }

        [JsonProperty("habilitado")]
        public bool Habilitado { get; set; }

        [JsonProperty("radicado")]
        public bool Radicado { get; set; }

        [JsonProperty("rutaDocumentos")]
        public string RutaDocumentos { get; set; }

        public override string ToString()
        {
            return $"{this.Nombre}";
        }
    }
}
