using Newtonsoft.Json;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class ProcesosJudicialesDTO
    {
        [JsonProperty("procesoId")]
        public int ProcesoId { get; set; }

        [JsonProperty("medioControl")]
        public string MedioControl { get; set; }

        [JsonProperty("juzgado")]
        public string Juzgado { get; set; }

        [JsonProperty("juridiccion")]
        public string Juridiccion { get; set; }

        [JsonProperty("apoderado")]
        public string Apoderado { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("demanda")]
        public string Demanda { get; set; }

        [JsonProperty("demandadoDemandante")]
        public string DemandadoDemandante { get; set; }


    }
}
