using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class DocumentoProcesoDTO
    {
        [JsonProperty("documentoProcesolId")]
        public int DocumentoProcesolId { get; set; }

        [JsonProperty("plantillaDocumentalId")]
        public decimal PlantillaDocumentalId { get; set; }

        [JsonProperty("procesoJudicialId")]
        public int ProcesoJudicialId { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [Required]
        [JsonProperty("documentoBase64")]
        public string DocumentoBase64 { get; set; } = string.Empty;

        [Required]
        [JsonProperty("identificador")]
        public string Identificador { get; set; } = string.Empty;
    }
}