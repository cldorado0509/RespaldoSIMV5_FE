

namespace SIM.Areas.QuejasAmbientales.Models
{
 
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ContestaQuejaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [JsonProperty("codigoTramite")]
        public decimal? CodigoTramite { get; set; }

        [JsonProperty("codigoDocumento")]
        public decimal? CodigoDocumento { get; set; }
    }
}