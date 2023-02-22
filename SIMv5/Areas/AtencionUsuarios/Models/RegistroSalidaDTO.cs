

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class RegistroSalidaDTO
    {
        [JsonProperty("idVisitaTercero")]
        public decimal IdVisitaTercero { get; set; }

        [JsonProperty("fechaSalida")]
        public DateTime? FechaSalida { get; set; }
    }
}