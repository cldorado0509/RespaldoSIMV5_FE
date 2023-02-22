

namespace SIM.Areas.QuejasAmbientales.Models
{

    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoNotificacionesDTO
    {
        [JsonProperty("idNotificacion")]
        public decimal IdNotificacion { get; set; }

        [JsonProperty("asociado")]
        public string Asociado { get; set; }

        [JsonProperty("fechaNotificacion")]
        public DateTime FechaNotificacion { get; set; }

        [JsonProperty("funcionarioElabora")]
        public string FuncionarioElabora { get; set; }

        [JsonProperty("personaN")]
        public string PersonaN { get; set; }

        [JsonProperty("personaJ")]
        public string PersonaJ { get; set; }

        [JsonProperty("tercero")]
        public string Tercero { get; set; }

        [JsonProperty("idForma")]
        public decimal? IdForma { get; set; }

        [JsonProperty("fechaVencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [JsonProperty("fechaFijacion")]
        public DateTime? FechaFijacion { get; set; }
    }
}