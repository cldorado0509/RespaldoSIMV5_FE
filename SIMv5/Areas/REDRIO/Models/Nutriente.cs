using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoNutriente
    {
        public List<Nutriente> result { get; set; }
    }

    public class Nutriente
    {
        public int idNutriente { get; set; }
        public decimal? Nitrogeno_total_kjeldahl { get; set; }

        public decimal? Fosforo_organico { get; set; }

        public decimal? Nitratos { get; set; }

        public decimal? Fosforo_total { get; set; }

        public decimal? Nitrogeno_organico { get; set; }

        public decimal? Nitritos { get; set; }

        public decimal? Fosfato { get; set; }

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get;  set; }

        public DateTime? Fecha_Muestra { get;  set; }

        public class ResponseNutriente
        {
            public Nutriente result { get; set; }
            public bool success { get; set; }
        }
    }

      public class ResponseNutriente
{
    public Nutriente result { get; set; }
    public bool success { get; set; }
}
}