namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.QRY_RESOLUCIONES")]
    public class QRY_RESOLUCIONES
    {
        [Key]
        [Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }
        public DateTime? FECHA_DIGITALIZARES { get; set; }
        public string RESOLUCION { get; set; }
        public string FECHA_RESOLUCION { get; set; }
        public string TIPO_RESOLUCION { get; set; }
        public string ABOGADO { get; set; }
        public string CM_RESOLUCION { get; set; }
    }
}









