namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_TARIFAS_MINIMAS")]
	public class TSIMTASA_TARIFAS_MINIMAS
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal ANO { get; set; }
		public decimal TARIFA { get; set; }
		public decimal TSIMTASAS_FACTOR_AMBIENTAL_ID { get; set; }
	}
}