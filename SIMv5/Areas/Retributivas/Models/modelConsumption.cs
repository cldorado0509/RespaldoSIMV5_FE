using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelConsumption
    {
        [JsonProperty("Id")]
        public decimal ID { get; set; }

        [JsonProperty("Mount")]
        public decimal MENSUAL { get; set; }

        [JsonProperty("Daily")]
        public decimal DIARIO { get; set; }

        [JsonProperty("IdTercero")]
        public decimal ID_TERCERO { get; set; }

        [JsonProperty("MaterialsId")]
        public decimal TSIMTASA_MATERIAS_PRIMA_ID { get; set; }

        [JsonProperty("MaterialsName")]
        public string MATERIALS_NAME { get; set; }

        [JsonProperty("UnitsId")]
        public decimal TSIMTASA_UNIDADES_ID { get; set; }

        [JsonProperty("UnitsName")]
        public string UNIDADES_NAME { get; set; }

    }
}