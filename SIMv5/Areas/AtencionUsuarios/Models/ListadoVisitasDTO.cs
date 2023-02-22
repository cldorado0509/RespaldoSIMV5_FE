
namespace SIM.Areas.AtencionUsuarios.Models
{
    using Independentsoft.Office.Odf;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ListadoVisitasDTO
    {
        [JsonProperty("IdVisitaTercero")]
        public decimal IdVisitaTercero { get; set; }

        [JsonProperty("IdTercero")]
        public decimal? IdTercero { get; set; }

        [JsonProperty("FechaIngreso")]
        
        public DateTime FechaIngreso { get; set; }

        [JsonProperty("FechaSalida")]
        public DateTime? FechaSalida { get; set; }

        [JsonProperty("Documento")]
        public decimal? Documento { get; set; }

        [JsonProperty("NombreCompleto")]
        public string NombreCompleto { get; set; }


        [JsonProperty("Carne")]
       
        public decimal Carne { get; set; }

        [JsonProperty("Empresa")]
        public string Empresa { get; set; }

        [JsonProperty("NombreDependencia")]
        public string NombreDependencia { get; set; }

        [JsonProperty("NombreFuncionario")]
        public string NombreFuncionario { get; set; }

        [JsonProperty("NombreMotivoVisita")]
        public string NombreMotivoVisita { get; set; }
    }
}