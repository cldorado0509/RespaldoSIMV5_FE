using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Data.Agua
{
    public class TSIMTASA_CUENTAS_TERCERO
	{
		public decimal ID { get; set; }
		public string TIPO_USO { get; set; }
		public string NO_RESOLUCION { get; set; }
		public DateTime FECHA_RESOLUCION { get; set; }
		public decimal ANOS_VIGENCIA { get; set; }
		public decimal LONGITUD { get; set; }
		public decimal LATITUD { get; set; }
		public decimal CAUDAL_AUTORIZADO { get; set; }
		public decimal TSIMTASAS_CUENCAS_ID { get; set; }
		public decimal ID_TERCERO { get; set; }
		public decimal ID_INSTALACION { get; set; }
		public decimal TSIMTASAS_CUENCAS_ID1 { get; set; }
	}
}