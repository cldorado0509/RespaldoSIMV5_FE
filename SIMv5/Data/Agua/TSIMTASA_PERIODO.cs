
namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("AGUA.TSIMTASA_PERIODO")]
	public class TSIMTASA_PERIODO
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal NO_PERIODO { get; set; }
		public DateTime? INICIA { get; set; }
		public DateTime? TERMINA { get; set; }
		public decimal TSIMTASA_QUINQUENO_ID { get; set; }
	}
}