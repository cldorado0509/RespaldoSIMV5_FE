using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Dynamics.Data
{
    public class ResponsableModel
    {
        [JsonProperty("ORDEN")]
        public long Orden { get; set; }

        [JsonProperty("RESPONSABLE")]
        public string Responsible { get; set; }
    }
}