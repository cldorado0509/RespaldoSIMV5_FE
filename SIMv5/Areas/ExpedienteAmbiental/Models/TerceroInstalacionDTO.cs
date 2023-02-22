namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    public class TerceroInstalacionDTO
    {
        [JsonProperty("idTercero")]
        public decimal? IdTercero { get; set; }

        [JsonProperty("documento")]
        public decimal? Documento { get; set; }

        [JsonProperty("rSocial")]
        public string RSocial { get; set; }

        [JsonProperty("telefono")]
        public decimal? Telefono { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

    }
}