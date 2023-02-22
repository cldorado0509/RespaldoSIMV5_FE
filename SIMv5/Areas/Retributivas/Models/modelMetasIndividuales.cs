using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelMetasIndividuales
    {

		[JsonProperty("Id_Meta_individual")]
		public decimal ID_META_INDIVIDUAL { get; set; }

		[JsonProperty("Meta")]
		public decimal META { get; set; }

		[JsonProperty("Carga_Obtenida")]
		public decimal CARGA_OBTENIDA { get; set; }

		[JsonProperty("Cumple_Meta")]
		public string CUMPLE_META { get; set; }

		[JsonProperty("Id_Tercero")]
		public decimal ID_TERCERO { get; set; }

		[JsonProperty("Name_Tercero")]
		public string NAME_TERCERO { get; set; }

		[JsonProperty("Periodo_Id")]
		public decimal PERIODO_ID { get; set; }

		[JsonProperty("Periodo")]
		public string PERIODO { get; set; }

		[JsonProperty("Parametro_Ambiental_Id")]
		public decimal PARAMETRO_AMBIENTAL_ID { get; set; }

		[JsonProperty("Parametro_Ambiental")]
		public string PARAMETRO_AMBIENTAL { get; set; }				
		
		[JsonProperty("Parametro_Ambiental_Abrev")]
		public string PARAMETRO_AMBIENTAL_ABREV { get; set; }		
		
		[JsonProperty("Factor_Regional_Id")]
		public decimal FACTOR_REGIONAL_ID { get; set; }

		[JsonProperty("Factor_Regional")]
		public decimal FACTOR_REGIONAL { get; set; }
	}
}