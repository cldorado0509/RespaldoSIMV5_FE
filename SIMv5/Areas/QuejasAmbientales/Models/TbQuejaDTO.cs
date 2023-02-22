

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

    public class TbQuejaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("codigoQueja")]
        public decimal CodigoQueja { get; set; }

        [JsonProperty("codigoAfectacion")]
        public decimal? CodigoAfectacion { get; set; }

        [JsonProperty("codigoRecurso")]
        public decimal? CodigoRecurso { get; set; }

        [JsonProperty("codigoUsuario")]
        public decimal? CodigoUsuario { get; set; }

        [JsonProperty("codigoComponente")]
        public decimal? CodigoComponente { get; set; }

        [JsonProperty("codigoMunicipio")]
        public decimal? CodigoMunicipio { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }

        [JsonProperty("fechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("recibe")]
        public string Recibe { get; set; }

        [JsonProperty("remitidoA")]
        public string RemitidoA { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("fechaTecnico")]
        public DateTime? FechaTecnico { get; set; }

        [JsonProperty("comentarios")]
        public string Comentarios { get; set; }

        [JsonProperty("fechaAbogado")]
        public DateTime? FechaAbogado { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("infractor")]
        public string Infractor { get; set; }

        [JsonProperty("telefonoInfractor")]
        public string TelefonoInfractor { get; set; }

        [JsonProperty("direccionInfractor")]
        public string DireccionInfractor { get; set; }

        [JsonProperty("fechaEstado")]
        public DateTime? FechaEstado { get; set; }

        [JsonProperty("funcionarioEstado")]
        public string FuncionarioEstado { get; set; }

        [JsonProperty("codigoTipoEstado")]
        public decimal? CodigoTipoEstado { get; set; }

        [JsonProperty("codigoFormaQueja")]
        public decimal? CodigoFormaQueja { get; set; }

        [JsonProperty("codigoTecnico")]
        public decimal? CodigoTecnico { get; set; }

        [JsonProperty("codigoAbogado")]
        public decimal? CodigoAbogado { get; set; }

        [JsonProperty("codigoCategoria")]
        public decimal? CodigoCategoria { get; set; }

        [JsonProperty("queja")]
        public decimal? Queja { get; set; }

        [JsonProperty("anno")]
        public string Anno { get; set; }

        [JsonProperty("radicadoVinculo")]
        public string RadicadoVinculo { get; set; }

        [JsonProperty("fechaRadicadoVinculo")]
        public DateTime? FechaRadicadoVinculo { get; set; }

        [JsonProperty("idExpediente")]
        public decimal? IdExpediente { get; set; }
    }
}