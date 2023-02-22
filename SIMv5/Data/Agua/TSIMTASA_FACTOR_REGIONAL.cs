
namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_FACTOR_REGIONAL")]
	public class TSIMTASA_FACTOR_REGIONAL
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal ANO { get; set; }
		public decimal FACTOR { get; set; }
		public string CUMPLE_META { get; set; }
		public double CARGA_OBTENIDA { get; set; }
		public string RESOLUCION { get; set; }
		public decimal TSIMTASA_TRAMOS_ID { get; set; }
		public decimal PARAMETROS_AMBIENTAL_ID { get; set; }
		public decimal TSIMTASA_PERIODOS_ID { get; set; }
	}
}