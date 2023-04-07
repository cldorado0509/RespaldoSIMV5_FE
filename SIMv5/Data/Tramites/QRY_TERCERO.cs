namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.QRY_TERCERO")]
    public class QRY_TERCERO
    {
        [Key]
        public decimal ID_TERCERO { get; set; }
        public decimal DOCUMENTO { get; set; }
        public decimal DIGITO { get; set; }
        public string TERCERO { get; set; }
        public string DIRECCION { get; set; }
        public string MUNICIPIO { get; set; }
        public decimal? ID_MUNICIPIO { get; set; }
        public string DEPARTAMENTO { get; set; }
        public decimal? ID_DEPARTAMENTO { get; set; }
        public decimal TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string NOMBRE1 { get; set; }
        public string NOMBRE2 { get; set; }
        public string APELLIDO1 { get; set; }
        public string APELLIDO2 { get; set; }
        public decimal TIPO { get; set; }
    }
}