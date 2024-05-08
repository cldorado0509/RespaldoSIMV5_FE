using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    public class ListadoDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("valor")]
        public string Valor { get; set; } = String.Empty;
    }
}