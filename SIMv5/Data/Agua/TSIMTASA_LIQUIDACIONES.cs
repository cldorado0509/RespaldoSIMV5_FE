

namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_LIQUIDACIONES")]
	public class TSIMTASA_LIQUIDACIONES
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public DateTime FECHA { get; set; }
		public decimal LIQUIDACION_DBO { get; set; }
		public decimal LIQUIDACION_SST { get; set; }
		public decimal REPORTES_ID { get; set; }
		public decimal DBOKGM { get; set; }
		public decimal SSTKGM { get; set; }
		public decimal LIQUIDACION { get; set; }
	}
}