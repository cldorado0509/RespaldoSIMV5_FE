

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class CompromisoDTO
    {
        [JsonProperty("codigoCompromiso")]
        public decimal CodigoCompromiso { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}