
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIM.Areas.QuejasAmbientales.Models
{
    public class TipoEstadoQuejaDTO
    {
        [JsonProperty("codTipoEstadoQueja")]
        public decimal CodTipoEstadoQueja { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("activo")]
        public string Activo { get; set; }
    }
}