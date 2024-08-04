using System;

namespace SIM.Areas.Contractual.DTO
{
    public class CertificadosDTO
    {
        public int IdCertificado { get; set; }
        public string Documento { get; set; }
        public string Tercero { get; set; }
        public DateTime FechaCertificado { get; set; }
        public string GeneradoPor { get; set; }
        public string ConActividades { get; set; }
        public int FuncionarioFirma { get; set; }
    }
}