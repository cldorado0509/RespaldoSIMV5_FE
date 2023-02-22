
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace SIM.Areas.QuejasAmbientales.Models
{
    public class InformeQuejaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("codInformeQueja")]
        public decimal CodInformeQueja { get; set; }

        [JsonProperty("codigoQueja")]
        public decimal? CodigoQueja { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("fecha")]
        public DateTime? Fecha { get; set; }

        [JsonProperty("tecnico")]
        public string Tecnico { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("fechaControl")]
        public DateTime? FechaControl { get; set; }

        [JsonProperty("fechaSeguimiento")]
        public DateTime? FechaSeguimiento { get; set; }

        [JsonProperty("responsable")]
        public string Responsable { get; set; }

        [JsonProperty("codTramite")]
        public decimal? CodTramite { get; set; }

        [JsonProperty("codDocumento")]
        public decimal? CodDocumento { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("componente")]
        public decimal? Componente { get; set; }

        [JsonProperty("idInforme")]
        public decimal? IdInforme { get; set; }
    }
}