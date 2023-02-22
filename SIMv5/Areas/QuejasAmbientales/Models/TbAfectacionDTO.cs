

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class TbAfectacionDTO
    {
        [JsonProperty("codigoAfectacion")]
        public decimal CodigoAfectacion { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("codigoRecurso")]
        public decimal? CodigoRecurso { get; set; }

        [JsonProperty("activo")]
        public string Activo { get; set; }
    }
}