

namespace SIM.Areas.QuejasAmbientales.Models
{

    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class EstadoQuejaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("codEstadoQueja")]
        public decimal CodEstadoQueja { get; set; }

        [JsonProperty("fechaEstado")]
        public DateTime? FechaEstado { get; set; }

        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }

        [JsonProperty("codQueja")]
        public decimal? CodQueja { get; set; }

        [JsonProperty("codTipoEstadoQueja")]
        public decimal? CodTipoEstadoQueja { get; set; }

        [JsonProperty("codFuncionario")]
        public decimal? CodFuncionario { get; set; }
    }
}