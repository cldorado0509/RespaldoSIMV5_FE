using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Encuestas.Models
{
    public class DefinicionPreguntaViewModel
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string NombreEncuesta { get; set; }
        public string Responsable { get; set; }
        public int TipoPregunta { get; set; }
        public string CampoRespuesta { get; set; }       
        public decimal Peso { get; set; }
        public int Orden { get; set; }
        public bool Requerida { get; set; }
        public bool Estado { get; set; }
        public string FechaGrid { get; set; }
        
        public DateTime Fecha { get; set; }
        public string Ayuda {get; set;}
        public System.Collections.Generic.List<DefinicionRespuestaViewModel> Respuestas { get; set; }
    }
}