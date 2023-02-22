using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Encuestas.Models
{
    public class InformacionBasicaEncuestaViewModel
    {
        public int Codigo { get; set; }
        public string NombreEncuesta { get; set; }
        public string Descripcion { get; set; }
        public string Responsable { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaGrid { get; set; }
        public string Formulario { get; set; }
        public string IdForm { get; set; }
    }
}