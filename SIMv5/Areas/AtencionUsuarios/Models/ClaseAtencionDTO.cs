

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ClaseAtencionDTO
    {
        [JsonProperty("idClaseAtencion")]
        public decimal IdClaseAtencion { get; set; }

        [JsonProperty("idTipoAtencion")]
        public decimal IdTipoAtencion { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("eliminado")]
        public string Eliminado { get; set; }

        [JsonProperty("activo")]
        public string Activo { get; set; }

        [JsonProperty("idFormulario")]
        public decimal? IdFormulario { get; set; }

        [JsonProperty("idVital")]
        public string IdVital { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }
    }
}