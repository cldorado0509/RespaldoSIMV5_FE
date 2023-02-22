

namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_GENERALIDADES")]
	public class TSIMTASA_GENERALIDADES
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal ID_TERCERO { get; set; }
		public decimal CIID { get; set; }
		public char ESP { get; set; }
		public decimal TSIMTASAS_TIPO_EMPRESA_ID { get; set; }
	}
}