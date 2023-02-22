

namespace SIM.Areas.QuejasAmbientales.Models
{
    
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    internal class ListadoVisitaQuejaDTO
    {
        [JsonProperty("codVisitaQueja")]
        public decimal CodVisitaQueja { get; set; }

        [JsonProperty("fechaVisita")]
        public DateTime? FechaVisita { get; set; }

        [JsonProperty("tecnico")]
        public string Tecnico { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("codigoTecnico")]
        public decimal? CodigoTecnico { get; set; }

        [JsonProperty("nombreObjeto")]
        public string NombreObjeto { get; set; }
    }
}