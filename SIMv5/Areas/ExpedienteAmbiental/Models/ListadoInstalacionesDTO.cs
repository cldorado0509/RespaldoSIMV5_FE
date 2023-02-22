namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    public class ListadoInstalacionesDTO
    {
        [JsonProperty("idInstalacion")]
        public decimal IdInstalacion { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }


        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

    }
}