namespace SIM.Areas.Contractual.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class RespuestaFirmaDTO
    {
        public bool IsSuccess { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Tercero { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public byte[] Archivo { get; set; }
    }
}