namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_CUENCAS")]
	public class TSIMTASA_CUENCAS
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal CODIGO { get; set; }
		public string NOMBRE { get; set; }
		public decimal CAUDAL { get; set; }
		public decimal LONGITUD { get; set; }
		public decimal AREA { get; set; }
		public decimal ID_MUNICIPIO { get; set; }
		public decimal TSIMTASA_TIPO_CUENCAS_ID { get; set; }
		public decimal TSIMTASA_TRAMOS_ID { get; set; }
	}
}