namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class ActoAsunto
    {
       
       
        [JsonProperty("tramiteId")]
        public decimal TramiteId { get; set; }

        [JsonProperty("documentoId")]
        public decimal DocumentoId { get; set; }

        [JsonProperty("unidadDocumental")]
        public string UnidadDocumental { get; set; }

        [JsonProperty("rutaDocumento")]
        public string RutaDocumento { get; set; }
    }
}