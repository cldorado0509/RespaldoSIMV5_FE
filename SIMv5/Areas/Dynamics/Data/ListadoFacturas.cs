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
        [JsonProperty("TERCERO")]
        public string Tercero { get; set; }
        [JsonProperty("FECHAFACT")]
        public DateTime Fechafact { get; set; }
        [JsonProperty("EMAIL")]
        public string Email { get; set; }
        [JsonProperty("MUNICIPIO")]
        public string Municipio { get; set; }
    }
}