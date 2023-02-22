using Newtonsoft.Json;

namespace SIM.Models
{
    public class TokenRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
       
        [JsonProperty("rememberMe")]
        public bool RememberMe { get; set; }
    }
}
