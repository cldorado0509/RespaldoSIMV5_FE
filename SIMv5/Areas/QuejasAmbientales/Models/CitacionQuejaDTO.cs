
namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class CitacionQuejaDTO
    {
        [JsonProperty("codCitacionQueja")]
        public decimal CodCitacionQueja { get; set; }

        [JsonProperty("codigoQueja")]
        public decimal? CodigoQueja { get; set; }

        [JsonProperty("codigoObjetoCitacion")]
        public decimal? CodigoObjetoCitacion { get; set; }

        [JsonProperty("fechaCitacion")]
        public DateTime? FechaCitacion { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("hora")]
        public string Hora { get; set; }

    }
}