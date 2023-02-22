
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIM.Areas.QuejasAmbientales.Models
{
    public class ListadoInformeQuejaDTO
    {
        [JsonProperty("codInformeQueja")]
        public decimal CodInformeQueja { get; set; }

        [JsonProperty("consecutivo")]
        public decimal? Consecutivo { get; set; }

        [JsonProperty("anio")]
        public decimal? Anio { get; set; }

        [JsonProperty("nroInforme")]
        public string NroInforme { get; set; }

        [JsonProperty("fechaElaboracion")]
        public DateTime FechaElaboracion { get; set; }

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

        [JsonProperty("IdInforme")]
        public decimal? IdInforme { get; set; }

        [JsonProperty("idDocumento")]
        public decimal? IdDocumento { get; set; }

        [JsonProperty("codTramite")]
        public decimal? CodTramite { get; set; }

        [JsonProperty("codDocumento")]
        public decimal? CodDocumento { get; set; }

    }
}