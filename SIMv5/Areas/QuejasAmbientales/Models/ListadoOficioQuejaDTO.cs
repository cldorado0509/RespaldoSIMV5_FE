

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoOficioQuejaDTO
    {
        [JsonProperty("idQuejaOficio")]
        public decimal IdQuejaOficio { get; set; }

        [JsonProperty("codTramite")]
        public decimal? CodTramite { get; set; }

        [JsonProperty("codDocumento")]
        public decimal? CodDocumento { get; set; }

        [JsonProperty("idQueja")]
        public decimal? IdQueja { get; set; }

        [JsonProperty("fechaAsociacion")]
        public DateTime? FechaAsociacion { get; set; }

        [JsonProperty("idDocumento")]
        public decimal IdDocumento { get; set; }
    }
}