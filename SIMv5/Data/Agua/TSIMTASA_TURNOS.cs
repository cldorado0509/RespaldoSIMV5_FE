namespace SIM.Data.Agua
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_TURNOS")]
	public class TSIMTASA_TURNOS
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public string NOMBRE { get; set; }
		public string DESCRIPCION { get; set; }
		public DateTime INICIA { get; set; }
		public DateTime TERMINA { get; set; }
		public decimal NO_OPERARIOS { get; set; }
		public decimal ID_TERCERO { get; set; }
	}
}