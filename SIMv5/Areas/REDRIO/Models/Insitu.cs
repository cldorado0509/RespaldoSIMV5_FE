using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoInsitu
    {
        public List<Insitu> result { get; set; }
    }

    public class Insitu
    {
        public int idInsitu { get; set; }
         public decimal?  OrpInsitu { get; set; }

        public decimal?  Oxigeno_disuelto {get; set;}

        public decimal?  Turbiedad { get; set; }

        public decimal?  Tem_agua {get; set;}    

        public decimal?  Temp_ambiente {get; set;}

        public decimal?  Conductiviidad_electrica { get; set; }

        public decimal?  PhInsitu {get; set;}    

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get;  set; }
        public DateTime? Fecha_Muestra { get;  set; }

    }

    public class ResponseInsitu
{
    public Insitu result { get; set; }
    public bool success { get; set; }
}
}
