
namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_QUINQUENO")]
	public class TSIMTASA_QUINQUENO
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public string DESCRIPCION { get; set; }
		public DateTime? INICIO { get; set; }
		public DateTime? TERMINA { get; set; }
		public string ACUERDO { get; set; }
	}
}