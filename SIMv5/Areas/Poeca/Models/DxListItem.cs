using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Poeca.Models
{
    public class DxListItem : ListItem
    {

        [JsonProperty("key")]
        public int Key { get; internal set; }
    }
}