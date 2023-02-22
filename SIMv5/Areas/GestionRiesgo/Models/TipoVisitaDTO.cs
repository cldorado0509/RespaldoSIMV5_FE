namespace SIM.Areas.GestionRiesgo.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class TipoVisitaDTO
    {
        [JsonProperty("idTipoVisita")]
        public decimal IdTipoVisita { get; set; }

        [JsonProperty("nombre")]
        [Required]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Nombre { get; set; }
    }
}