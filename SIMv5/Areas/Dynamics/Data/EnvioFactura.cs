namespace SIM.Areas.Dynamics.Data
{
    public class EnvioFactura
    {
        public string IdFact { get; set; }
        public string Mail { get; set; }
        public string Tercero { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}