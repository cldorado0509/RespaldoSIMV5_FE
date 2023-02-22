

namespace SIM.Areas.QuejasAmbientales.Models
{
   
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    internal class ListadoResolucionesDTO
    {
        [JsonProperty("idResolucion")]
        public decimal IdResolucion { get; set; }

        [JsonProperty("codResolucion")]
        public decimal? CodResolucion { get; set; }

        [JsonProperty("anio")]
        public decimal Anio { get; set; }

        [JsonProperty("fechaElaboracion")]
        public DateTime? FechaElaboracion { get; set; }

        [JsonProperty("nombreFuncionario")]
        public string NombreFuncionario { get; set; }

        [JsonProperty("nombreTipoResolucion")]
        public string NombreTipoResolucion { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("idDocumento")]
        public decimal IdDocumento { get; set; }
    }
}