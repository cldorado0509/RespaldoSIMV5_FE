using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Pqrsd.Models
{
    public struct datosConsulta
    {
        public int numRegistros;
        public IEnumerable<dynamic> datos;
    }
}