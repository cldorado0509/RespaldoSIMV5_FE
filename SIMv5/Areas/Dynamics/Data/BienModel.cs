﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Dynamics.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class BienModel
    {
        [JsonProperty("CODIGO")]
        public string AssetId { get; set; }
        [JsonProperty("NOMBREBIEN")]
        public string ASSETNAME { get; set; }
        [JsonProperty("ESTADOBIEN")]
        public string ESTADO { get; set; }
        [JsonProperty("PERSONABIEN")]
        public string RESPONSIBLE { get; set; }
    }
}