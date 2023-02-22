

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoQuejasDTO
    {
        [JsonProperty("CodigoQueja")]
        public decimal CodigoQueja { get; set; }

        [JsonProperty("NroQueja")]
        public decimal? NroQueja { get; set; }

        [JsonProperty("Anno")]
        public string Anno { get; set; }

        [JsonProperty("FechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("Radicado")]
        public string Radicado { get; set; }

        [JsonProperty("Asunto")]
        public string Asunto { get; set; }

        [JsonProperty("Recibe")]
        public string Recibe { get; set; }

        [JsonProperty("NombreRecurso")]
        public string NombreRecurso { get; set; }

        [JsonProperty("NombreAfectacion")]
        public string NombreAfectacion { get; set; }

        [JsonProperty("CodigoAfectacion")]
        public decimal? CodigoAfectacion { get; set; }

        [JsonProperty("CodigoRecurso")]
        public decimal? CodigoRecurso { get; set; }

        [JsonProperty("IdExpediente")]
        public decimal? IdExpediente { get; set; }

        [JsonProperty("NombreTipoEstadoQueja")]
        public string NombreTipoEstadoQueja { get; set; }
    }
}