using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Seguridad.Models
{
    public class REDRIOModel
    {
        public List<Dato> Datos { get; set; }

        // Otras propiedades del modelo
    }

    public class Dato
    {
        public int Id { get; set; }
        public string CM { get; set; }
        public string CedulaNit { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public string Telefono { get; set; }
    }
}