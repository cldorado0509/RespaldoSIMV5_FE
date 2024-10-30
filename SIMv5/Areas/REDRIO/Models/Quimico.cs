using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoQuimico
    {
        public List<Quimico> result { get; set; }
    }

    public class Quimico
    {
        public int idQuimico { get; set; }
         public decimal?  sustanciaActivaAzulMetileno { get; set; }

        public decimal?  Grasa_Aceite {get; set;}

        public decimal?  Db05 { get; set; }

        public decimal?  Dq0 {get; set;}    

        public decimal?  HierroTotal {get; set;} 

        public decimal?  Sulfatos {get; set;} 

        public decimal?  Sulfuros {get; set;} 

        public decimal?  Cloruros {get; set;} 

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime Fecha_actualizacion { get;  set; }
        public DateTime? Fecha_Muestra { get;  set; }
        
    }

    public class ResponseQuimico
{
    public Quimico result { get; set; }
    public bool success { get; set; }
}
}
