using System;

namespace SIM.Areas.Dynamics.Data
{
    public class DatosFacturaHeader
    {
        public string Factura { get; set; }
        public DateTime FechaElabora { get; set; }
        public DateTime FechaVence { get; set; }
        public decimal? ValorFactura { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }

    }
}