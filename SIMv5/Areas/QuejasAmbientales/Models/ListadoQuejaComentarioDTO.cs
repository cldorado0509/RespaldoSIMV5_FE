

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ListadoQuejaComentarioDTO
    {
        [JsonProperty("idQuejaComentario")]
        public decimal IdQuejaComentario { get; set; }

        [JsonProperty("comentario")]
        public string Comentario { get; set; }

        [JsonProperty("idFuncionario")]
        public decimal? IdFuncionario { get; set; }

        [JsonProperty("fechaComentario")]
        public DateTime? FechaComentario { get; set; }

        [JsonProperty("idQueja")]
        public decimal? IdQueja { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set;}
    }
}
