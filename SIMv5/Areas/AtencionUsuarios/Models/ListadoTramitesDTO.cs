

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoTramitesDTO
    {
        [JsonProperty("idAtencionTramite")]
        public decimal IdAtencionTramite { get; set; }

        [JsonProperty("nombreProceso")]
        public string NombreProceso { get; set; }

        [JsonProperty("CodTramite")]
        public decimal CodTramite { get; set; }

        [JsonProperty("fechaInicio")]
        public DateTime? FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        public DateTime? FechaFin { get; set; }


        [JsonProperty("estado")]
        public decimal? Estado { get; set; }
    }
}