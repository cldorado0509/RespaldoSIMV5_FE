
namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ListadoQuejasDTO
    {
        [JsonProperty("idQuejaAtencion")]
        public decimal IdQuejaAtencion { get; set; }

        [JsonProperty("fechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("CodigoQueja")]
        public decimal? CodigoQueja { get; set; }

        [JsonProperty("Queja")]
        public decimal? Queja { get; set; }

        [JsonProperty("ano")]
        public string Ano { get; set; }

        [JsonProperty("nombreMunicipio")]
        public string NombreMunicipio { get; set; }

        [JsonProperty("nombreRecurso")]
        public string NombreRecurso { get; set; }

        [JsonProperty("nombreAfectacion")]
        public string NombreAfectacion { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }

        [JsonProperty("infractor")]
        public string Infractor { get; set; }
    }
}