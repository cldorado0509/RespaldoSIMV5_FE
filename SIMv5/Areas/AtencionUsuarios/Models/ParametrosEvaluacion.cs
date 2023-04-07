using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.AtencionUsuarios.Models
{
    public class ParametrosEvaluacion
    {
        public decimal NumeroProfesionales { get; set; }
        public int NumeroVisitas { get; set; }
        public decimal HorasInforme { get; set; }
        public decimal DuracionVisita { get; set; }
        public string Unidad { get; set; }
    }
}