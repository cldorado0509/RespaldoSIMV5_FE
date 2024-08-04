using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.Contractual.DTO
{
    public class RequestCertificadoDTO
    {
        [Required]
        public int Idter { get; set; }
        public string Mail { get; set; } = string.Empty;
        public bool Actividades { get; set; } = false;
        public int? UsuarioGenera { get; set; }
    }
}