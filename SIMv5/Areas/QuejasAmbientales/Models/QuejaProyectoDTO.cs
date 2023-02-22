
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace SIM.Areas.QuejasAmbientales.Models
{
    public class QuejaProyectoDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("codQuejaProyecto")]
        public decimal CodQuejaProyecto { get; set; }

        [JsonProperty("codProyecto")]
        public decimal? CodProyecto { get; set; }

        [JsonProperty("codQueja")]
        public decimal? CodQueja { get; set; }

        [JsonProperty("origenCm")]
        public string OrigenCm { get; set; }
    }
}