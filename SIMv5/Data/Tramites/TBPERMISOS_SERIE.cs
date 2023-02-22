namespace SIM.Data.Tramites
{
    using DocumentFormat.OpenXml.Drawing.Charts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.TBPERMISOS_SERIE")]
    public class TBPERMISOS_SERIE
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODSERIE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODFUNCIONARIO { get; set; }
        public string PC { get; set; }
        public string PA { get; set; }
        public string PM { get; set; }
        public string PE { get; set; }
        public string PP { get; set; }
    }
}