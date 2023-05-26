using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIM.Data.Tramites
{
    /// <summary>
    /// 
    /// </summary>
    [Table("TRAMITES.QRY_VERTEMPORALES")]
    public class QRY_VERTEMPORALES
    {
        [Key]
        public decimal CODFUNCIONARIO { get; set; }
        public string NOMBRES { get; set; }
        public string ACTIVO { get; set; }
    }
}