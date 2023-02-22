
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIM.Areas.QuejasAmbientales.Models
{
    public class ListadoTramitesQuejaDTO
    {
        [JsonProperty("codigoTramiteExpediente")]
        public decimal CodigoTramiteExpediente { get; set; }

        [JsonProperty("CodTramite")]
        public decimal CodTramite { get; set; }

        [JsonProperty("fechaIni")]
        public DateTime? Fecha { get; set; }

        [JsonProperty("tipoTramite")]
        public string TipoTramite { get; set; }

        [JsonProperty("estado")]
        public decimal? Estado { get; set; }

        [JsonProperty("comentarios")]
        public string Comentarios { get; set; }


    }
}