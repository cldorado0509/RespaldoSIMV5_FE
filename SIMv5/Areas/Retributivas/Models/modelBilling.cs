using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelBilling
    {

		[JsonProperty("Id_Reporte")]
		public decimal ID_REPORTE { get; set; }

		[JsonProperty("Vertimiento_Id")]
		public decimal VERTIMIENTO_ID { get; set; }

		[JsonProperty("Vertimiento")]
		public string VERTIMIENTO { get; set; }

		[JsonProperty("Agno")]
		public decimal AGNO { get; set; }

		[JsonProperty("Mes_Id")]
		public decimal MES_ID { get; set; }

		[JsonProperty("Mes")]
		public string MES { get; set; }

		[JsonProperty("FR_Tramo")]
		public decimal FR_TRAMO { get; set; }
		
		[JsonProperty("Meta_Tramo")]
		public decimal Meta_TRAMO { get; set; }
		
		[JsonProperty("Meta_Individuao")]
		public decimal Meta_Individual { get; set; }

		[JsonProperty("FR_Individual")]
		public decimal FR_INDIVIDUAL { get; set; }

		[JsonProperty("Tarifa_Minima_dbo")]
		public decimal TARIFA_MINIMA_DBO { get; set; }
		
		[JsonProperty("Tarifa_Minima_sst")]
		public decimal TARIFA_MINIMA_SST { get; set; }
		
		[JsonProperty("Report_dbo")]
		public decimal REPORT_DBO { get; set; }
		
		[JsonProperty("Report_sst")]
		public decimal REPORT_SST { get; set; }
		
		[JsonProperty("Billing_dbo")]
		public decimal BILLING_DBO { get; set; }
		
		[JsonProperty("Billing_sst")]
		public decimal BILLING_SST { get; set; }

		[JsonProperty("Parametro_AmbientaL")]
		public string PARAMETRO_AMBIENTAL { get; set; }
		
		[JsonProperty("Id_Parametro_AmbientaL")]
		public decimal ID_PARAMETRO_AMBIENTAL { get; set; }

		

	}
}