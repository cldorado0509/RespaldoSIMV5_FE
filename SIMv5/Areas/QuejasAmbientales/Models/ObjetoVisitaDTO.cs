

namespace SIM.Areas.QuejasAmbientales.Models
{
    
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ObjetoVisitaDTO
    {
        [JsonProperty("codigoObjetoVisita")]
        public decimal CodigoObjetoVisita { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}