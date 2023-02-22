using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Data.Agua
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("AGUA.TSIMTASA_REPORTES")]

	public class TSIMTASA_REPORTES
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public decimal ID { get; set; }
		public decimal CANTIDAD_VERTIMIENTOS { get; set; }
		public decimal CAUDAL_PROMEDIO { get; set; }
		public decimal HORAS_VERTIMIENTO { get; set; }
		public decimal DIAS_VERTIMIENTOS { get; set; }
		public decimal MES { get; set; }
		public decimal ANO { get; set; }
		public decimal REPORTE_DBO { get; set; }
		public decimal REPORTE_SST { get; set; }
		public byte[] ADJUNTOS { get; set; }
		public decimal CUMPLE { get; set; }
		public decimal CODTRAMITE { get; set; }
		public decimal CODDOCUMENTO { get; set; }
		public decimal TSIMTASA_CUENCAS_TERCERO_ID { get; set; }
		public decimal TSIMTASA_ESTADO_REPORTE_ID { get; set; }
		public decimal TSIMTASA_TIPO_REPORTE_ID { get; set; }

	}
}