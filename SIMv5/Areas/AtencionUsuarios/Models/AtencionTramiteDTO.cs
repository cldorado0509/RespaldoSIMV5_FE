

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
    public class AtencionTramiteDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idAtencionTramite")]
        public decimal IdAtencionTramite { get; set; }

        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("codigoTramite")]
        public decimal CodigoTramite { get; set; }
    }
}