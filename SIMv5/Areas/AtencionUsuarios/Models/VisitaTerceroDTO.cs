



namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class VisitaTerceroDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idVisitaTercero")]

        public decimal IdVisitaTercero { get; set; }


        [JsonProperty("fechaIngreso")]
        [Required(ErrorMessage = "La fecha es requerida!")]
        public DateTime FechaIngreso { get; set; }

        [JsonProperty("fechaSalida")]
        public DateTime? FechaSalida { get; set; }

        [JsonProperty("carne")]
        [Required(ErrorMessage = "El carné es requerido!")]
        public decimal Carne { get; set; }

        [JsonProperty("dependencia")]
        public decimal? Dependencia { get; set; }

        [JsonProperty("motivoVisita")]
        [Required(ErrorMessage = "El motivo de la visita es requerido!")]
        public decimal MotivoVisita { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("entrega")]
        public string Entrega { get; set; }

        [JsonProperty("codFuncionario")]
        public decimal? CodFuncionario { get; set; }

        [JsonProperty("idPersona")]
        public decimal IdPersona { get; set; }

    }
}