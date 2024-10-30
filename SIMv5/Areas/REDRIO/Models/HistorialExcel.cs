using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoHistorialExcel
    {
        public List<HistorialExcel> result { get; set; }
    }

    public class HistorialExcel
    {
        public int idHistorialExcel { get; set; }
        public DateTime Fecha_cargue { get; set; }

        public string nombreUsuario {get; set;} 

        public string Url {get; set;} 

        public int? IdCampaña {get; set;}


        public DateTime? Fecha_actualizacion { get; internal set; }
    }

   public class ResponseHistorialExcel
{
    public HistorialExcel result { get; set; }
    public bool success { get; set; }
}   
}
