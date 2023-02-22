using Newtonsoft.Json;

namespace SIM.Areas.Pqrsd.Models
{
    public class ArchivoAnexoDTO
    {
        [JsonProperty("IdPQRSD")]
        public string IdPQRSD { get; set; }

        [JsonProperty("Anexo")]
        public byte[] Anexo { get; set; }

        [JsonProperty("extension")]
        public string extension { get; set; }
    }
}