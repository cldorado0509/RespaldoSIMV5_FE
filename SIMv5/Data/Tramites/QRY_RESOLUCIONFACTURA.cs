namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.QRY_RESOLUCIONFACTURA")]
    public class QRY_RESOLUCIONFACTURA
    {
        [Key]
        [Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }
        public string PAGO_TOTAL { get; set; }
        public string FACTURA_ASIGNADA { get; set; }
        public string RADICADO { get; set; }
        public string FECHA_RADICADO { get; set; }
        public string ASUNTO { get; set; }

    }
}