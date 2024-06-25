using Newtonsoft.Json;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class DocumentoAnexoDTO
    {
        [JsonProperty("procesDocumentoAnexoId")]
        public int ProcesDocumentoAnexoId { get; set; }

        [JsonProperty("procesoJudicialId")]
        public int ProcesoJudicialId { get; set; }

        [JsonProperty("tipo")]
        public int Tipo { get; set; }

        [JsonProperty("documento")]
        public string Documento { get; set; }

        [JsonProperty("bytesDoc")]
        public byte[] BytesDoc { get; set; } = new byte[0];
    }
}