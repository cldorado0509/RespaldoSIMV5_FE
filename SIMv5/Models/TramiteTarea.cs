using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Models
{
    public class TramiteTarea
    {
        public int CodTramite { get; set; }
        
        public DateTime? FechaIncioTramite { get; set; }
        public string  Proceso { get; set; }
        public string Tarea { get; set; }
        public DateTime FechaIniciaTarea { get; set; }
        public string TipoTarea { get; set; }
        public string Funcionario { get; set; }
        public string QueDeboHacer { get; set; }
        public string Vital { get; set; }

        public decimal Orden { get; set; }

        public decimal CodFuncionario { get; set; }

        public bool Propietario { get; set; }
        public bool TramiteAbierto { get; set; }  
    }
}