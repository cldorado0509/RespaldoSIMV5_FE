
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

    public class AtencionDocumentoDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idAtencionDocumento")]
        public decimal IdAtencionDocumento { get; set; }

        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("codTramite")]
        public decimal? CodTramite { get; set; }

        [JsonProperty("codDocumento")]
        public decimal? CodDocumento { get; set; }

        [JsonProperty("idDocumento")]
        public decimal? IdDocumento { get; set; }

    }
}