using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoReporteLaboratorio
    {
        public List<ReporteLaboratorio> result { get; set; }
    }

    

    public class ReporteLaboratorio
    {
        public int idReporte { get; set; }

        
        public int? IdResultadoCampo { get; set; }

        
        public int? IdCampaña { get; set; }

        
        public int? IdEstacion { get; set; }

        public int? IdMuestraCompuesta { get; set; }

        public DateTime? fecha_creacion { get;  set; }

        public DateTime? fecha_actualizacion { get;  set; }

        public Estacion estacion { get; set; }
        public ResultadoCampo resultadoCampo { get; set; }
        public Campaña campaña { get; set; }
        public MuestraCompuesta muestraCompuesta { get; set; }
    }

    public class ResponseReporteLaboratorio
{
    public ReporteLaboratorio result { get; set; }
    public bool success { get; set; }
}   
}
