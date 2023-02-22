
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIM.Areas.QuejasAmbientales.Models
{
    public class EditarQuejaTerceroDTO
    {
        [JsonProperty("idQuejaTercero")]
        public decimal IdQuejaTercero { get; set; }

        [JsonProperty("codigoQueja")]
        public decimal? CodigoQueja { get; set; }

        [JsonProperty("idTercero")]
        public decimal? IdTercero { get; set; }

        [JsonProperty("idInstalacion")]
        public decimal? IdInstalacion { get; set; }

        [JsonProperty("idTipoTerceroQueja")]
        public decimal? IdTipoTerceroQueja { get; set; }

        [JsonProperty("fechaTerceroQueja")]
        public DateTime? FechaTerceroQueja { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("especial")]
        public string Especial { get; set; }

        [JsonProperty("documentoN")]
        public decimal? DocumentoN { get; set; }
    }
}