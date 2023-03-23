namespace SIM.Areas.AdministracionDocumental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class Metadato
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("unidadId")]
        [Required]
        public decimal unidadId { get; set; }

        [JsonProperty("nombre", Required = Required.Always)]
        [Required]
        public string Nombre { get; set; }

        [JsonProperty("tipo")]
        [Required]
        public byte Tipo { get; set; }

        [JsonProperty("longitud")]
        public long Longitud { get; set; }

        [JsonProperty("obligatorio")]
        public bool Obligatorio { get; set; }

        [JsonProperty("valorDefecto")]
        public string ValorDefecto { get; set; }

        [JsonProperty("mostrar")]
        public bool Mostrar { get; set; }

        [JsonProperty("mostrarEnGrid")]
        public bool MostrarEnGrid { get; set; }

        [JsonProperty("orden")]
        public int Orden { get; set; }

        [JsonProperty("secuenciaId")]
        public decimal? SecuenciaId { get; set; }


        [JsonProperty("ListadoId")]
        public int? ListadoId { get; set; }

    }
}