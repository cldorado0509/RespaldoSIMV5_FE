
namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ReporteQuejaDTO
    {
        [JsonProperty("nroQueja")]
        public decimal? NroQueja { get; set; }

        [JsonProperty("anno")]
        public string Anno { get; set; }

        [JsonProperty("fechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }

        [JsonProperty("recibe")]
        public string Recibe { get; set; }

        [JsonProperty("nombreRecurso")]
        public string NombreRecurso { get; set; }

        [JsonProperty("nombreAfectacion")]
        public string NombreAfectacion { get; set; }


        [JsonProperty("rSocial")]
        public string RSocial { get; set; }

        [JsonProperty("nombreInstalacion")]
        public string NombreInstalacion { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("nombreTipoTercero")]
        public string NombreTipoTercero { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("documentoN")]
        public decimal? DocumentoN { get; set; }

        [JsonProperty("nombreForma")]
        public string NombreForma { get; set; }

        [JsonProperty("municipio")]
        public string Municipio { get; set; }

        [JsonProperty("nombreAbogado")]
        public string NombreAbogado { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("comentarios")]
        public string Comentarios { get; set; }


        [JsonProperty("fechaTercero")]
        public DateTime? FechaTercero { get; set; }
    }
}