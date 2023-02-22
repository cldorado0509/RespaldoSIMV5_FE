using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Facturacion.Models
{
    public class FactCont
    {
        public int NO_FACTURA { get; set; }
        public DateTime FECHA { get; set; }
        public DateTime FECHA_VENCE { get; set; }
        public string DESCRIPCION { get; set; }
        public string CODIGO_CONCEPTO { get; set; }
        public string DESCRP_CONCEPTO { get; set; }
        public int VALOR_CONCEPTO { get; set; }
        public int CODIGO_COMERCIAL { get; set; }
        public string NIT { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public int CUENTAS_VENCIDAS { get; set; }
        public int VALOR_VENCIDO { get; set; }
        public string CIUDAD { get; set; }
        public string DEPARTAMENTO { get; set; }
    }
}