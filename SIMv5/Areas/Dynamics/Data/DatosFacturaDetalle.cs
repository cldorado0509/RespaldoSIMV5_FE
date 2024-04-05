namespace SIM.Areas.Dynamics.Data
{
    public class DatosFacturaDetalle
    {
        public string Factura { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
        public decimal Linea { get; set; }
        public decimal Valor { get; set; }
    }
}