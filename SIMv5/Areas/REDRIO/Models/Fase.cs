using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoFases
    {
        public List<Fase> result { get; set; }
    }

    

    public class TipoFase
    {
        public int idTipoFase { get; set; }
        public string nombreTipoFase { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public DateTime? fecha_actualizacion { get; set; }
    }
}
