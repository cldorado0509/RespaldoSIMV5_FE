namespace SIM.Areas.ExpedienteAmbiental.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ProcesoDTO
    {
        [JsonProperty("codProceso")]
        [Key]
        public decimal CODPROCESO { get; set; }

        [JsonProperty("nombre")]
        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        [JsonProperty("descripcion")]
        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        [JsonProperty("tiempo")]
        public decimal? TIEMPO { get; set; }

        [JsonProperty("valido")]
        public int? VALIDO { get; set; }

        [JsonProperty("informacion")]
        [StringLength(4000)]
        public string INFORMACION { get; set; }

        [JsonProperty("activo")]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        [JsonProperty("talaPoda")]
        [StringLength(1)]
        public string TALAPODA { get; set; }

        [JsonProperty("radicadoActual")]
        public int RADICADO_ACTUAL { get; set; }

        [JsonProperty("iniciaAutomatico")]
        [Required]
        [StringLength(1)]
        public string INICIA_AUTOMATICO { get; set; }

        [JsonProperty("marcaTarea")]
        [StringLength(1)]
        public string S_MARCATAREA { get; set; }

        [JsonProperty("prioridad")]
        [StringLength(1)]
        public string S_PRIORITARIO { get; set; }

        [JsonProperty("colorMarca")]
        [StringLength(20)]
        public string S_COLORMARCA { get; set; }
    }
}