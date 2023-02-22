using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIM.Areas.General.Models;
using SIM.Areas.EnCicla.Models;
using SIM.Data.EnCicla;
using SIM.Data.General;

namespace SIM.Areas.EnCicla.Models
{
    public class DATOSPRESTAMO
    {
        public int idEstacion { get; set; }
        public string cedula { get; set; }
        public string codigoBicicleta { get; set; }
        public string reporteNovedad { get; set; }
        public string observacionesNovedad { get; set; }
    }

    public class DATOSUSUARIO
    {
        public NATURAL natural { get; set; }
        public TERCERO_ROL terceroRol { get; set; }
        public string mensaje { get; set; }
    }
}