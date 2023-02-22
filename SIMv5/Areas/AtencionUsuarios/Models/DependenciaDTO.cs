


namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class DependenciaDTO
    {
        [JsonProperty("idDepencencia")]
        public decimal IdDepencencia { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("eliminado")]
        public string Eliminado { get; set; }

        [JsonProperty("activo")]
        public string Activo { get; set; }

        [JsonProperty("codDepencencia")]
        public decimal? CodDepencencia { get; set; }

        [JsonProperty("codEntidad")]
        public string CodEntidad { get; set; }

        [JsonProperty("dInicio")]
        public DateTime? DInicio { get; set; }

        [JsonProperty("dFin")]
        public DateTime? DFin { get; set; }

        [JsonProperty("idDepencenciaPadre")]
        public decimal? IdDepencenciaPadre { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("codOficina")]
        public string CodOficina { get; set; }

        [JsonProperty("ordenadorGasto")]
        public string OrdenadorGasto { get; set; }

        [JsonProperty("preContractual")]
        public string PreContractual { get; set; }

        [JsonProperty("descripcion2")]
        public string Descripcion2 { get; set; }
    }
}