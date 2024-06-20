using Newtonsoft.Json;
using System;

namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class ProcuraduriaDTO
    {
        [JsonProperty("procuraduriaId")]
        public decimal ProcuraduriaId { get; set; }


        [JsonProperty("nombre")]
        public string Nombre { get; set; } = String.Empty;
    }
}