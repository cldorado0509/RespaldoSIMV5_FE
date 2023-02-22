using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelMetasGrupales
    {

		[JsonProperty("id_meta_grupal")]
		public decimal ID_META_GRUPAL { get; set; }

		[JsonProperty("meta")]
		public decimal META { get; set; }

		[JsonProperty("periodo")]
		public decimal PERIODO { get; set; }

		[JsonProperty("parametro_ambiental_id")]
		public decimal PARAMETRO_AMBIENTAL_ID { get; set; }		
		
		[JsonProperty("parametro_ambiental")]
		public string PARAMETRO_AMBIENTAL { get; set; }

		[JsonProperty("tsimtasa_tramo_id")]
		public decimal TSIMTASA_TRAMO_ID { get; set; }		
		
	}
}