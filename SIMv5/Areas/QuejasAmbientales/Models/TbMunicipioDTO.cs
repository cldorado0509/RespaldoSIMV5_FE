

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class TbMunicipioDTO
    {

        [JsonProperty("codigoMunicipio")]
        public decimal CodigoMunicipio { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("codigoDane")]
        public string CodigoDane { get; set; }

        [JsonProperty("coorX")]
        public decimal? CoorX { get; set; }

        [JsonProperty("coorY")]
        public decimal? CoorY { get; set; }

        [JsonProperty("coorZ")]
        public decimal? CoorZ { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("rataCrecimiento")]
        public decimal? RataCrecimiento { get; set; }

        [JsonProperty("codigoDepartamento")]
        public decimal CodigoDepartamento { get; set; }

        [JsonProperty("idDivipola")]
        public decimal? IdDivipola { get; set; }

        [JsonProperty("mailNotifica")]
        public string MailNotifica { get; set; }
    }
}