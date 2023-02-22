
namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ChequeoDTO
    {
        [JsonProperty("idChequeo")]
        public decimal IdChequeo { get; set; }

        [JsonProperty("orden")]
        public decimal Orden { get; set; }

        [JsonProperty("chequeo")]
        public string Chequeo { get; set; }

        [JsonProperty("idClaseAtencion")]
        public decimal? IdClaseAtencion { get; set; }

        [JsonProperty("formato")]
        public string Formato { get; set; }

        [JsonProperty("obligatorio")]
        public string Obligatorio { get; set; }
    }
}