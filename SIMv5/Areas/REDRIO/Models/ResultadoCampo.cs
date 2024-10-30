using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoResultadoCampo
    {
        public List<ResultadoCampo> result { get; set; }
    }

    

    public class ResultadoCampo
    {
        public int idCampo { get; set; }
        public string Hora { get; set; } = string.Empty;

        public decimal? TempAmbiente { get; set; } 

        public decimal? TempAgua { get; set; } 

        public decimal? Ph { get; set; } 

        public decimal? Od { get; set; } 

        public decimal? Cond { get; set; } 

        public decimal? Orp { get; set; } 

        public decimal? Turb { get; set; } 

        public string Tiempo { get; set; } = string.Empty;

        public string Apariencia { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Olor { get; set; } = string.Empty;

        public string Altura { get; set; } = string.Empty;

        public string H1 { get; set; } = string.Empty;

        public string H2 { get; set; } = string.Empty;

        public string Observacion { get; set; } = string.Empty;

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get;  set; }
        public DateTime? Fecha_Muestra { get;  set; }

    }
      public class ResponseCampo
{
    public ResultadoCampo result { get; set; }
    public bool success { get; set; }
}
}
