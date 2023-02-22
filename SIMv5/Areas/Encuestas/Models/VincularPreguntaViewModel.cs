using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Encuestas.Models
{
    public class VincularPreguntaViewModel
    {
        public int CodigoPregunta { get; set; }
        public int Peso { get; set; }
        public int Orden { get; set; }
        public bool Requerida { get; set; }
    }
}