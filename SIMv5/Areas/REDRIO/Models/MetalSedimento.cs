using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoMetalSedimental
    {
        public List<MetalSedimental> result { get; set; }
    }

    public class MetalSedimental
    {
        public int idMetalSedimental { get; set; }
        public decimal?  Cadmio_sedimentable { get; set; }

        public decimal?  Cobre_sedimentable { get; set; }
        
        public decimal?  Cromo_sedimentable {get; set;}   

        public decimal?  Mercurio_sedimentable {get; set;}   

        public decimal?  Plomo_sedimentable {get; set;}

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get;  set; }
        public DateTime? Fecha_Muestra { get;  set; }

    }

    public class ResponseMetalSedimental
{
    public MetalSedimental result { get; set; }
    public bool success { get; set; }
}
}
