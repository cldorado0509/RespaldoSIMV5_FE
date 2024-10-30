
     using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoMuestraCompuesta
    {
        public List<MuestraCompuesta> result { get; set; }
    }

    

    public class MuestraCompuesta
    {
        public int idMuestraCompuesta { get; set; }
        public int? IdInsitu { get; set; }

        
        public int? idNutriente { get; set; }

        
        public int? IdQuimico { get; set; }

        
        public int? IdFisico { get; set; }

        
        public int? IdMetalAgua { get; set; }

        public int? IdMetalSedimental { get; set; }

        
        public int? IdBiologico { get; set; }
  
        public DateTime? fecha_creacion { get;  set; }

        public DateTime? fecha_actualizacion { get;  set; }

        public Insitu insitu { get; set; }

        public Biologico biologico { get; set; }
        public Fisico fisico { get; set; }
        public Quimico quimico { get; set; }
        public MetalAgua metalAgua { get; set; }
        public MetalSedimental MetalSedimental { get; set; }
        public Nutriente nutriente { get; set; }
         public DateTime? Fecha_Muestra { get;  set; }



    }

      public class ResponseMuestraCompuesta
    {
        public MuestraCompuesta result { get; set; }
        public bool success { get; set; }
    }
}

