using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Retributivas.Models
{
    public class modelReports
    {

		[JsonProperty("Id_Reporte")]
		public decimal ID_REPORTE { get; set; }

		[JsonProperty("Vertimiento_Id")]
		public decimal VERTIMIENTO_ID { get; set; }

		[JsonProperty("Vertimiento")]
		public string VERTIMIENTO { get; set; }
		
		[JsonProperty("Nick")]
		public string NICK { get; set; }

		[JsonProperty("Agno")]
		public decimal AGNO { get; set; }

		[JsonProperty("Mes_Id")]
		public decimal MES_ID { get; set; }

		[JsonProperty("Mes")]
		public string MES { get; set; }

		[JsonProperty("No_Descargas_Dia")]
		public decimal NO_DESCARGAS_DIA { get; set; }

		[JsonProperty("Horas_Descargas_Dia")]
		public decimal HORAS_DESCARGAS_DIA { get; set; }

		[JsonProperty("Dias_Descargas_Mes")]
		public decimal DIAS_DESCARGAS_MES { get; set; }

		[JsonProperty("Id_Tercero")]
		public decimal ID_TERCERO { get; set; }

		[JsonProperty("Name_Tercero")]
		public string NAME_TERCERO { get; set; }

		[JsonProperty("Nit_Tercero")]
		public long NIT_TERCERO { get; set; }

		[JsonProperty("dv")]
		public byte DV { get; set; }

		[JsonProperty("Estado_Reporte_Id")]
		public decimal ESTADO_REPORTE_ID { get; set; }

		[JsonProperty("Estado_Reporte")]
		public string ESTADO_REPORTE { get; set; }

		[JsonProperty("Tipo_Reporte_Id")]
		public decimal TIPO_REPORTE_ID { get; set; }

		[JsonProperty("Tipo_Reporte")]
		public string TIPO_REPORTE { get; set; }		
		
		[JsonProperty("Tipo_Descarga_Id")]
		public decimal TIPO_DESCARGA_ID { get; set; }

		[JsonProperty("Tipo_Descarga")]
		public string TIPO_DESCARGA { get; set; }

		[JsonProperty("Tipo_Agua_residual_Id")]
		public decimal TIPO_AGUA_RESIDUAL_ID { get; set; }

		[JsonProperty("Tipo_Agua_Residual")]
		public string TIPO_AGUA_RESIDUAL { get; set; }

		[JsonProperty("dbo")]
		public decimal DBO { get; set; }

		[JsonProperty("sst")]
		public decimal SST { get; set; }
		
		[JsonProperty("billing_dbo")]
		public decimal BILLING_DBO { get; set; }

		[JsonProperty("billing_sst")]
		public decimal BILLING_SST { get; set; }
		
		[JsonProperty("dbokgm")]
		public decimal DBOKGM { get; set; }

		[JsonProperty("sstkgm")]
		public decimal SSTKGM { get; set; }

		[JsonProperty("total_billing")]
		public decimal TOTAL_BILLING { get; set; }

		[JsonProperty("Caudal")]
		public decimal CAUDAL { get; set; }

		[JsonProperty("Radicado")]
		public decimal RADICADO { get; set; }




	}
}