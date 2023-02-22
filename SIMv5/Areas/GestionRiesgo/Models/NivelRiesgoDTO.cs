namespace SIM.Areas.GestionRiesgo.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class NivelRiesgoDTO
    {
        [JsonProperty("idNivelRiesgo")]
        public decimal IdNivelRiesgo { get; set; }

        [JsonProperty("nombre")]
        [Required]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Nombre { get; set; }

    }
}