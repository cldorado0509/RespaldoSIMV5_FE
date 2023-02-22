namespace SIM.Areas.Pqrsd.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ListadosDTO
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("valor")]
        [Required]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string VALOR { get; set; } = String.Empty;

    }
}