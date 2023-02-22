

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
    public class QuejaComentarioDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idQuejaComentario")]
        public decimal IdQuejaComentario { get; set; }

        [JsonProperty("comentario")]
        public string Comentario { get; set; }

        [JsonProperty("idFuncionario")]
        public decimal? IdFuncionario { get; set; }

        [JsonProperty("idTercero")]
        public decimal? IdTercero { get; set; }

        [JsonProperty("fechaComentario")]
        public DateTime? FechaComentario { get; set; }

        [JsonProperty("idQueja")]
        public decimal? IdQueja { get; set; }
    }
}