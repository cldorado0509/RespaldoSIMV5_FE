namespace SIM.Areas.DesarrolloEconomico.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class Categoria
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("nombre", Required = Required.Always)]
        [Required]
        public string Nombre { get; set; }


        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("estado")]
        public bool Estado { get; set; }

        public override string ToString()
        {
            return $"{this.Nombre}";
        }

    }
}