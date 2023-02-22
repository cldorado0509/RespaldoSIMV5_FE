

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class TerceroNaturalDTO
    {
        [JsonProperty("idTercero")]

        public decimal IdTercero { get; set; }

        [JsonProperty("documentoN")]
        public decimal? DocumentoN { get; set; }

        [JsonProperty("primerApellido")]
        public string PrimerApellido { get; set; }

        [JsonProperty("segundoApellido")]
        public string SegundoApellido { get; set; }

        [JsonProperty("primerNombre")]
        public string PrimerNombre { get; set; }

        [JsonProperty("segundoNombre")]
        public string SegundoNombre { get; set; }

        [JsonProperty("genero")]
        public string Genero { get; set; }

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