using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelBasinFactory
    {

		[JsonProperty("Id_Cuenca_Tercero")]
		public decimal ID_CUENCAS_TERCERO { get; set; }

		[JsonProperty("Cuenca")]
		public string CUENCA { get; set; }
		
		[JsonProperty("Nick")]
		public string NICK { get; set; }

		[JsonProperty("Cuenca_Id")]
		public decimal CUENCA_ID { get; set; }

		[JsonProperty("No_Resolucion")]
		public string NO_RESOLUCION { get; set; }

		[JsonProperty("Fecha_Resolucion")]
		public DateTime FECHA_RESOLUCION { get; set; }

		[JsonProperty("Tipo_Descarga")]
		public string TIPO_DESCARGA { get; set; }

		[JsonProperty("Tipo_Agua_Residual")]
		public string TIPO_AGUA_RESIDUAL { get; set; }

		[JsonProperty("Tipo_Descarga_Id")]
		public decimal TIPO_DESCARGA_ID { get; set; }

		[JsonProperty("Tipo_Agua_residual_Id")]
		public decimal TIPO_AGUA_RESIDUAL_ID { get; set; }

		[JsonProperty("Agnos_Vigencia")]
		public decimal AGNOS_VIGENCIA { get; set; }

		[JsonProperty("Longitud")]
		public decimal LONGITUD { get; set; }

		[JsonProperty("Latitud")]
		public decimal LATITUD { get; set; }

		[JsonProperty("Caudal")]
		public decimal CAUDAL { get; set; }

		[JsonProperty("Id_Tercero")]
		public decimal ID_TERCERO { get; set; }

		[JsonProperty("Id_Instalacion")]
		public decimal ID_INSTALACION { get; set; }

	}
}