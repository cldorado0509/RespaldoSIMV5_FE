
namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_CUENCAS_TERCERO")]
	public class TSIMTASA_CUENCAS_TERCERO
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal TIPO_DESCARGA { get; set; }
		public decimal TIPO_AGUA_RESIDUAL { get; set; }
		public string NO_RESOLUCION { get; set; }
		public string NICK { get; set; }
		public DateTime FECHA_RESOLUCION { get; set; }
		public decimal ANOS_VIGENCIA { get; set; }
		public decimal LONGITUD { get; set; }
		public decimal LATITUD { get; set; }
		public decimal CAUDAL_AUTORIZADO { get; set; }
		public decimal ID_TERCERO { get; set; }
		public decimal ID_INSTALACION { get; set; }
		public decimal TSIMTASA_CUENCAS_ID1 { get; set; }
	}
}