using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM.Areas.Encuestas.Models
{
    public class SolucionEncuestaViewModel
    {
        public int CodigoPregunta  {get; set;}
        public int? CodigoRespuesta  {get; set;}
        public string ValorTexto  {get; set;}
        public int? ValorNumero  {get; set;}
        public DateTime? ValorFecha  {get; set;}
    }
}
