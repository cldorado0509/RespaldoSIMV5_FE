using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class ResponsableTareaDTO
    {
        [JsonProperty("tareaResponsableId")]
        public int TareaResponsableId { get; set; }

        [JsonProperty("codTarea")]
        public int CodTarea { get; set; }

        [JsonProperty("codFuncionario")]
        public int CodFuncionario { get; set; }

        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }

        [JsonProperty("codTareaPadre")]
        public int? CodTareaPadre { get; set; }
    }
}