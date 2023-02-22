using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelProduction
    {

		[JsonProperty("Id")]
		public decimal ID { get; set; }

		[JsonProperty("Mensual")]
		public decimal MENSUAL { get; set; }

		[JsonProperty("Diario")]
		public decimal DIARIO { get; set; }

		[JsonProperty("IdTercero")]
		public decimal ID_TERCERO { get; set; }

		[JsonProperty("ProductosId")]
		public decimal TSIMTASA_PRODUCTOS_ID { get; set; }

		[JsonProperty("ProductosName")]
		public string PRODUCTOS_NAME { get; set; }

		[JsonProperty("UnidadesId")]
		public decimal TSIMTASA_UNIDADES_ID { get; set; }

		[JsonProperty("UnidadesName")]
		public string UNIDADES_NAME { get; set; }
	}
}