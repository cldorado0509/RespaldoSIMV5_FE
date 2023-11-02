using SIM.Data.Seguridad;
using System;

namespace SIM.Areas.GestionDocumental.Models
{
    public class ListadoMasivos
    {
        public decimal ID { get; set; }
        public string TEMA { get; set; }
        public DateTime D_FECHA { get; set; }
        public decimal CANTIDAD_FILAS { get; set; }
        public string ESTADO { get; set; }
        public string IDSOLICITUD { get; set; }
        public string CODTRAMITE { get; set; }
        public string ENVIACORREO { get; set; }
        public string MENSAJE { get; set; }
        public decimal FUNCIONARIOFIRMA { get; set; }
        public decimal FUNCIONARIOELABORA { get; set; }
    }
}