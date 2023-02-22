

namespace SIM.Data.Control
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("CONTROL.INFORMACION_INDUSTRIA")]

	public class INFORMACION_INDUSTRIA
	{
		[Key]
		[Column(Order = 1)]
		public decimal ID_TERCERO { get; set; }
		[Key]
		[Column(Order = 0)]
		public decimal ID_INSTALACION { get; set; }
		public decimal? ID_SECTOR { get; set; }
		public decimal? N_TIEMPO_OPERACION_HD { get; set; }
		public decimal? N_TIEMPO_OPERACION_DS { get; set; }
		public decimal? N_TIEMPO_OPERACION_SM { get; set; }
		public decimal? N_TIEMPO_OPERACION_MA { get; set; }
		public decimal? N_CAP_INSTALADA_TON_DIA { get; set; }
		public decimal? N_MAX_PROD_DIARIA_TON_DIA { get; set; }
		public decimal? N_PRODUCCION_HORARIA { get; set; }
		public decimal? N_NRO_EMPLEADOS { get; set; }
		public decimal? N_NRO_TURNOS { get; set; }
		public decimal? N_TURNOS_DIA { get; set; }
		public string OBSERVACIONES { get; set; }

		[Key]
		[Column(Order = 2)]
		public decimal ID_VISITA { get; set; }
	}
}