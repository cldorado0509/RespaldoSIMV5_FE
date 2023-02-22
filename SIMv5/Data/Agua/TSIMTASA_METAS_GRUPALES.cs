
namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_METAS_GRUPALES")]
	public class TSIMTASA_METAS_GRUPALES
	{
		public decimal ID { get; set; }
		public decimal META { get; set; }
		public decimal PERIODO { get; set; }
		public decimal PARAMETRO { get; set; }
		public decimal TSIMTASA_TRAMO_ID { get; set; }
	}
}