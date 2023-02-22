namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoSolicitudesDTO
    {
        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("codigoSolicitud")]
        public decimal CodigoSolicitud { get; set; }

        [JsonProperty("CodigoTramite")]
        public decimal? CodigoTramite { get; set; }

        [JsonProperty("codigoProyecto")]
        public decimal CodigoProyecto { get; set; }

        [JsonProperty("idAtencionProyecto")]
        public decimal IdAtencionProyecto { get; set; }

        [JsonProperty("codigoTipoSolicitud")]
        public decimal CodigoTipoSolicitud { get; set; }

        [JsonProperty("CM")]
        public string CM { get; set; }

        [JsonProperty("nombreTipoSolicitud")]
        public string NombreTipoSolicitud { get; set; }

        [JsonProperty("conexo")]
        public string Conexo { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}