using Newtonsoft.Json;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class MunicipioDTO
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("codigoDane")]
        public string CodigoDane { get; set; }

    }
}
