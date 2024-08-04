namespace SIM.Areas.Contractual.DTO
{
    public class CertificadoDTO
    {
        public bool isSucceded { get; set; }
        public string Message { get; set; } = string.Empty;

        public byte[] Certificado { get; set; }
    }
}