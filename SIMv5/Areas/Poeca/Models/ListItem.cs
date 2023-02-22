using Newtonsoft.Json;

namespace SIM.Areas.Poeca.Models
{
    public class ListItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}