namespace SIM.Areas.AdministracionDocumental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class UnidadTablaRetencion
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }


        [JsonProperty("unidadId")]
        public decimal UnidadId { get; set; }

        [JsonProperty("serieDocumental")]
        public string SerieDocumental { get; set; }

        [JsonProperty("subSerieDocumental")]
        public string SubSerieDocumental { get; set; }

        [JsonProperty("unidadDocumental")]
        public string UnidadDocumental { get; set; }

        [JsonProperty("asignada")]
        public bool Asignada { get; set; }

    }
}