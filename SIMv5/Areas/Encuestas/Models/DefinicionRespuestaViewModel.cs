using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Encuestas.Models
{
    public class DefinicionRespuestaViewModel
    {
        public int Codigo { get; set; }
        public int CodigoPregunta { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }        
    }
}