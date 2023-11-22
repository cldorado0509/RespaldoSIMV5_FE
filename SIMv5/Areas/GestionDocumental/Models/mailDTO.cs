using Newtonsoft.Json;

namespace SIM.Areas.GestionDocumental.Models
{
    public class mailDTO
    {
        [JsonProperty("to")]
        public string to { get; set; }
        //[JsonProperty("subject")]
        //public string subject { get; set; }
        //[JsonProperty("body")]
        //public string body { get; set; }
        //[JsonProperty("smtpServer")]
        //public string smtpServer { get; set; }
        //[JsonProperty("smtpPort")]
        //public string smtpPort { get; set; }
        //[JsonProperty("userPass")]
        //public string userPass { get; set; }
        //[JsonProperty("fromMail")]
        //public string fromMail { get; set; }
        //[JsonProperty("attachement")]
        //public byte[] attachement { get; set; }
        //[JsonProperty("attName")]
        //public string attName { get; set; }

    }
}
