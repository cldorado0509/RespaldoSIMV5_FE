namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class ProyectoCM
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("coordenadaX")]
        public float CoordenadaX { get; set; }

        [JsonProperty("coordenadaY")]
        public float CoordenadaY { get; set; }

        [JsonProperty("coordenadaZ")]
        public float CoordenadaZ { get; set; }

        [JsonProperty("asuntos")]
        public IEnumerable <Asunto> Asuntos { get; set; }
    }
}