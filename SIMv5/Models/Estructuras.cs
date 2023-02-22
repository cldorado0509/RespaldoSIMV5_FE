using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Models
{
    public class DatosVisita
    {
        public int IDTRAMITE { get; set; }
        public int IDVISITA { get; set; }
        public string EMPRESA { get; set; }
        public string DIRECCION { get; set; }
        public string MUNICIPIO { get; set; }
        public string NOMBRE_INSTALACION { get; set; }
        public string TELEFONO_INSTALACION { get; set; }
        public string CM { get; set; }
        public string REPRESENTANTE_LEGAL { get; set; }
        public long? NRO_DOCUMENTO { get; set; }
        public int? ID_TERCERO { get; set; }
        public int? ID_INSTALACION { get; set; }
        public string QUEJA { get; set; }
        public int? ANO { get; set; }
        public string ABOGADO { get; set; }
    }
}