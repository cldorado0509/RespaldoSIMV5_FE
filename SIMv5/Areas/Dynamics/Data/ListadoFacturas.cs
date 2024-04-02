using Newtonsoft.Json;
using System;

namespace SIM.Areas.Dynamics.Data
{
    public class ListadoFacturas
    {
        [JsonProperty("FACTURA")]
        public string Factura { get; set; }
        [JsonProperty("DOCUMENTO")]
        public string Documento { get; set; }
        [JsonProperty("NOMBRE")]
        public string Nombre { get; set; }
        [JsonProperty("FECHAFACT")]
        public DateTime Fechafact { get; set; }
        [JsonProperty("EMAIL")]
        public string Email { get; set; }
        [JsonProperty("CIUDAD")]
        public string Ciudad { get; set; }
    }
}