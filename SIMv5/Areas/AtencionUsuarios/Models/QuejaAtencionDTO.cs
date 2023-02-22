

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class QuejaAtencionDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idQuejaAtencion")]
        public decimal IdQuejaAtencion { get; set; }

        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("idQueja")]
        public decimal IdQueja { get; set; }
    }
}