using Newtonsoft.Json;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class DocumentoAportadoDTO
    {

        /// <summary>
        /// Nombre del Documento
        /// </summary>
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}