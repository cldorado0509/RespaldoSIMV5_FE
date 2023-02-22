

namespace SIM.Areas.AtencionUsuarios.Models
{
    
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoDocumentosDTO
    {
        [JsonProperty("idAtencionDocumento")]
        public decimal IdAtencionDocumento { get; set; }

        [JsonProperty("idDocumento")]
        public decimal IdDocumento { get; set; }

        [JsonProperty("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [JsonProperty("nombreSerie")]
        public string NombreSerie { get; set; }


    }
}