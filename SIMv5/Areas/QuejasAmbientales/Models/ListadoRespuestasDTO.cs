

namespace SIM.Areas.QuejasAmbientales.Models
{
  
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoRespuestasDTO
    {
        [JsonProperty("codContestaQueja")]
        public decimal CodContestaQueja { get; set; }

        [JsonProperty("codigoQueja")]
        public decimal? CodigoQueja { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("fecha")]
        public DateTime? Fecha { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }
    }
}