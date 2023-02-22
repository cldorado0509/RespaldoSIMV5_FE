

namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_TIPO_CUENCAS")]
	public class TSIMTASA_TIPO_CUENCAS
	{
		public decimal ID { get; set; }
		public string NOMBRE { get; set; }
	}
}