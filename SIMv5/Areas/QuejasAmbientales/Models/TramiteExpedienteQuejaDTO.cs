
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
    public class TramiteExpedienteQuejaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idTramiteExpediente")]
        public decimal IdTramiteExpediente { get; set; }

        [JsonProperty("codTramite")]
        public decimal CodTramite { get; set; }

        [JsonProperty("codQueja")]
        public decimal CodQueja { get; set; }
    }
}