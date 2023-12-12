namespace SIM.Areas.GestionDocumental.Models
{
    public class ResponseMassiveDTO
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public byte[] responseFile { get; set; }
    }
}