
namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ListaDependenciaDTO
    {
        [JsonProperty("codFuncionario")]
        public decimal CodFuncionario { get; set; }


        [JsonProperty("IdDependencia")]
        public decimal IdDependencia { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}