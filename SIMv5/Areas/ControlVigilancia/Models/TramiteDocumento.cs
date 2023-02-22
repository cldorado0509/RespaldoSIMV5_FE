namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    public class TramiteDocumento
    {
        [JsonProperty("idTramite")]
        public decimal IdTramite { get; set; }

        [JsonProperty("idDocumento")]
        public decimal IdDocumento { get; set; }

    }
}