using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AbogadoDTO
    {
        /// <summary>
        /// Identifica el abogado
        /// </summary>
        [JsonProperty("idAbogado")]
        public int IdAbogado { get; set; }


        /// <summary>
        /// Nombre del Abogado
        /// </summary>
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}