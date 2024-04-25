using Newtonsoft.Json;

namespace SIM.Areas.Seguridad.Models
{
    public class ResponseUser : ApplicationUser
    {
        public ResponseUser(string userName) : base(userName) { }
        [JsonProperty("Token")]
        public string Token { get; set; }
        [JsonProperty("ExpiresIn")]
        public int ExpiresIn { get; set; }
    }
}