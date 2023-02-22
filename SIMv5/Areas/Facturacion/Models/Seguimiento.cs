using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Facturacion.Models
{
    public class Seguimiento
    {
        public bool CalculoExito { get; set; }
        public string Mensaje { get; set; }
        public string TotalPagar { get; set; }
        public byte[] Soporte { get; set; }
    }
}