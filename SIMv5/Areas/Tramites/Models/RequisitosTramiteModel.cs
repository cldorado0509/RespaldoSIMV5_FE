using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Tramites.Models
{
    public class RequisitosTramiteModel
    {
        public int ID_REQUISITO { get; set; }
        public string REQUISITO { get; set; }
        public int ID_TRAMITE { get; set; }
        public int ID_ESTADO { get; set; }
        public string NOMBRE_ESTADO { get; set; }
        public string FORMATO { get; set; }
        public string OBLIGATORIO { get; set; }
        public int ID_INSTALACION { get; set; }

        public List<Models.RequisitosTramiteModel> ListReTra { get; set; }
    }
}