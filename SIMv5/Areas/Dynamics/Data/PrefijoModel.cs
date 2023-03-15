using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Dynamics.Data
{
    public class PrefijoModel
    {
        [JsonProperty("ORDEN")]
        public long Orden { get; set; }

        [JsonProperty("PREFIJO")]
        public string Prefijo { get; set; }
    }
}