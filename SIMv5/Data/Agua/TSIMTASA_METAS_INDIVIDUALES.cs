
namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_METAS_INDIVIDUALES")]
	public class TSIMTASA_METAS_INDIVIDUALES
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal META { get; set; }
		public decimal CARGA_OBTENIDA { get; set; }
		public string CUMPLE_META { get; set; }
		public decimal ID_TERCERO { get; set; }
		public decimal TSIMTASA_PERIODO_ID { get; set; }
		public decimal TSIMTASA_PARAMETROS_AMBIENTAL_ID { get; set; }
		public decimal FACTOR_REGIONAL { get; set; }
	}
}