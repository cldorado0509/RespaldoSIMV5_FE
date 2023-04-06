using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.AtencionUsuarios.Models
{
    public class ParametrosCalculo
    {
        [JsonProperty("IdCalculo")]
        public decimal IdCalculo { get; set; }
        [JsonProperty("Sueldos")]
        public decimal Sueldos { get; set; }
        [JsonProperty("Viajes")]
        public decimal Viajes { get; set; }
        [JsonProperty("Otros")]
        public decimal Otros { get; set; }
        [JsonProperty("Admin")]
        public decimal Admin { get; set; }
        [JsonProperty("Costo")]
        public decimal Costo { get; set; }
        [JsonProperty("Topes")]
        public decimal Topes { get; set; }
        [JsonProperty("Valor")]
        public decimal Valor { get; set; }
        [JsonProperty("Publicacion")]
        public decimal Publicacion { get; set; }
    }
}