using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIM.Areas.TalaPoda.Models
{
    public class GridTalaPodaViewModel
    {
        //public int Consecutivo { get; set; }
        public decimal IdIndividuo { get; set; }
        public string Especie { get; set; }
        public string Estado { get; set; }
        public decimal Accion { get; set; }
        public decimal Excluir { get; set; }
        
    }
}