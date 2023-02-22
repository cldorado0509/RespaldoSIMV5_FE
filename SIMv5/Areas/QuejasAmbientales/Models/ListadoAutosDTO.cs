
namespace SIM.Areas.QuejasAmbientales.Models
{

    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ListadoAutosDTO
    {

        [JsonProperty("idAuto")]
        public decimal IdAuto { get; set; }

        [JsonProperty("codAuto")]
        public decimal CodAuto { get; set; }

        [JsonProperty("anio")]
        public decimal Anio { get; set; }

        [JsonProperty("fechaElaboracion")]
        public DateTime FechaElaboracion { get; set; }

        [JsonProperty("nombreFuncionario")]
        public string NombreFuncionario { get; set; }

        [JsonProperty("nombreTipoAuto")]
        public string NombreTipoAuto { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("idDocumento")]
        public decimal IdDocumento { get; set; }
    }
}