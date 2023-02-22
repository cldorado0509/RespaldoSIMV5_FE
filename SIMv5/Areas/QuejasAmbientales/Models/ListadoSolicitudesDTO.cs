namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoSolicitudesDTO
    {
        [JsonProperty("codigoQueja")]
        public decimal CodigoQueja { get; set; }

        [JsonProperty("codigoSolicitud")]
        public decimal CodigoSolicitud { get; set; }

        [JsonProperty("codigoProyecto")]
        public decimal CodigoProyecto { get; set; }

        [JsonProperty("codQuejaProyecto")]
        public decimal CodQuejaProyecto { get; set; }

        [JsonProperty("codigoTipoSolicitud")]
        public decimal CodigoTipoSolicitud { get; set; }

        [JsonProperty("CM")]
        public string CM { get; set; }

        [JsonProperty("nombreTipoSolicitud")]
        public string NombreTipoSolicitud { get; set; }

        [JsonProperty("conexo")]
        public string Conexo { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("idExpDoc")]
        public decimal? IdExpDoc { get; set; }
    }
}