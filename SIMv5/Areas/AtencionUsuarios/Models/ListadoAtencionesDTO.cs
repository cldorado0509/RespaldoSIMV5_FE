

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class ListadoAtencionesDTO
    {
        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("fechaAtencion")]
        public DateTime FechaAtencion { get; set; }

        [JsonProperty("idUsuario")]
        public decimal IdUsuario { get; set; }

        [JsonProperty("rSocial")]
        public string RSocial { get; set; }

        [JsonProperty("tipoAtencion")]
        public string TipoAtencion { get; set; }

        [JsonProperty("claseAtencion")]
        public string ClaseAtencion { get; set; }

        [JsonProperty("formaAtencion")]
        public string FormaAtencion { get; set; }

        [JsonProperty("atendidoPor")]
        public string AtendidoPor { get; set; }

        [JsonProperty("detalle")]
        public string Detalle { get; set; }

        [JsonProperty("fechaCompromiso")]
        public DateTime? FechaCompromiso { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }




    }
}