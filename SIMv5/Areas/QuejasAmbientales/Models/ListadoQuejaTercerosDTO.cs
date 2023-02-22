
namespace SIM.Areas.QuejasAmbientales.Models
{
    
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ListadoQuejaTercerosDTO
    {
        [JsonProperty("idTipoTerceroQueja")]
        public decimal? IdTipoTerceroQueja { get; set; }

        [JsonProperty("idInstalacion")]
        public decimal? IdInstalacion { get; set; }

        [JsonProperty("idTercero")]
        public decimal? IdTercero { get; set; }

        [JsonProperty("idQuejaTercero")]
        public decimal IdQuejaTercero { get; set; }

        [JsonProperty("idQuejaAtencion")]
        public decimal? IdQuejaAtencion { get; set; }

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

    }
}