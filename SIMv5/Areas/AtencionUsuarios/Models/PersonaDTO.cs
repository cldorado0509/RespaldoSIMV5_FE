
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

    public class PersonaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idPersona")]
        public decimal IdPersona { get; set; }

        [JsonProperty("idTipoDocumento")]
        public decimal? IdTipoDocumento { get; set; }

        [JsonProperty("documento")]
        public decimal Documento { get; set; }

        [JsonProperty("primerNombre")]
        public string PrimerNombre { get; set; }

        [JsonProperty("segundoNombre")]
        public string SegundoNombre { get; set; }

        [JsonProperty("primerApellido")]
        public string PrimerApellido { get; set; }

        [JsonProperty("segundoApellido")]
        public string SegundoApellido { get; set; }

        [JsonProperty("genero")]
        public string Genero { get; set; }

        [JsonProperty("telefono")]
        public decimal? Telefono { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }
    }
}