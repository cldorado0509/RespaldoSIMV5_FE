

namespace SIM.Areas.QuejasAmbientales.Models
{
    
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoEstadoQuejaDTO
    {
        [JsonProperty("codEstadoQueja")]
        public decimal CodEstadoQueja { get; set; }

        [JsonProperty("fechaEstado")]
        public DateTime? FechaEstado { get; set; }

        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }

        [JsonProperty("codQueja")]
        public decimal? CodQueja { get; set; }

        [JsonProperty("nombreTipoEstadoQueja")]
        public string NombreTipoEstadoQueja { get; set; }

        [JsonProperty("codFuncionario")]
        public decimal? CodFuncionario { get; set; }
    }
}