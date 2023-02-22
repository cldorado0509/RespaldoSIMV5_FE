
namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_PRODUCTOS")]
	public class TSIMTASA_PRODUCTOS
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public string NOMBRE { get; set; }
		public string DESCRIPCION { get; set; }
	}
}