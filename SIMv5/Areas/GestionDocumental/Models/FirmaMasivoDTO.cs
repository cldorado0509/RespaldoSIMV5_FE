namespace SIM.Areas.GestionDocumental.Models
{
    public class FirmaMasivoDTO
    {
        public string IdSolicitud { get; set; }
        public decimal CodFuncionario { get; set; }
        public string Comentario { get; set; }
        public bool Firmado { get; set; }
    }
}