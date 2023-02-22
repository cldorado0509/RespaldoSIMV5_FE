

namespace SIM.Areas.QuejasAmbientales.Models
{
   
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class TerceroJuridicoDTO
    {
        [JsonProperty("idTercero")]

        public decimal IdTercero { get; set; }

        [JsonProperty("documentoN")]
        public decimal? DocumentoN { get; set; }

        [JsonProperty("telefono")]
        public decimal? Telefono { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("nombreCompleto")]
        public string NombreCompleto { get; set; }
    }
}