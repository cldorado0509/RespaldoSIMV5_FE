using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoBiologico
    {
        public List<Biologico> result { get; set; }
    }

    public class Biologico
    {
        public int idBiologico { get; set; }
        public decimal?  Escherichia_coli_npm { get; set; }

        public decimal?  Escherichia_coli_ufc {get; set;}

        public decimal?  Indice_biologico { get; set; }

        public decimal?  Coliformes_totales_ufc {get; set;}   
        
        public decimal?  Coliformes_totales_npm {get; set;}   

        public decimal?  Riquezas_algas {get; set;}   

        public string  ClasificacionIBiologico {get; set;} = string.Empty;

        public string  Observaciones {get; set;} = string.Empty;

        public DateTime? Fecha_creacion { get; internal set; }

        public DateTime? Fecha_actualizacion { get; internal set; }
        public DateTime? Fecha_Muestra { get;  set; }

    }

    public class ResponseBiologico
{
    public Biologico result { get; set; }
    public bool success { get; set; }
}
}
