using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoMetalAgua
    {
        public List<MetalAgua> result { get; set; }
    }

    public class MetalAgua
    {
        public int idMetalAgua { get; set; }
        public decimal?  Cadmio { get; set; }

        public decimal?  Niquel {get; set;}

        public decimal?  Cobre { get; set; }

        public decimal?  Mercurio {get; set;}   
        
        public decimal?  Cromo {get; set;}   

        public decimal?  Plomo {get; set;}
        
        public decimal?  Cromo_hexavalente {get; set;}  

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get;  set; }

        public DateTime? Fecha_Muestra { get;  set; }

    }

    public class ResponseMetalAgua
{
    public MetalAgua result { get; set; }
    public bool success { get; set; }
}
}
