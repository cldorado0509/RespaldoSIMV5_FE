
namespace SIM.Areas.QuejasAmbientales.Models
{
   
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class TipoTerceroQuejaDTO
    {
        [JsonProperty("idTipoTerceroQueja")]
        public decimal IdTipoTerceroQueja { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}