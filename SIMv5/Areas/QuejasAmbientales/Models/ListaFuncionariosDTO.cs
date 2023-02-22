

namespace SIM.Areas.QuejasAmbientales.Models
{
   
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListaFuncionariosDTO
    {
        [JsonProperty("codFuncionario")]
        public decimal CodFuncionario { get; set; }

        [JsonProperty("idDependencia")]
        public decimal IdDependencia { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }
    }
}