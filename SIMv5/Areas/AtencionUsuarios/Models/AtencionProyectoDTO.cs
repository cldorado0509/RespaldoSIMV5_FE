
namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class AtencionProyectoDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("idAtencionProyecto")]
        public decimal IdAtencionProyecto { get; set; }

        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("codigoProyecto")]
        public decimal? CodigoProyecto { get; set; }

        [JsonProperty("codigoSolicitud")]
        public decimal? CodigoSolicitud { get; set; }

    }
}