namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.QRY_INFORMESTECNICOS")]
    public class QRY_INFORMESTECNICOS
    {
        public string VALOR { get; set; }
        public string ASUNTO { get; set; }
        [Column("FECHA RADICADO")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FECHA_RADICADO { get; set; }
        public string RADICADO { get; set; }
        [Column("FACTURA ASIGNADA")]
        public string FACTURA_ASIGNADA { get; set; }
        [Column("CONTROL SEGUIMIENTO AMBIENTAL")]
        public string CONTROL_SEGUIMIENTO_AMBIENTAL { get; set; }
        public string TECNICO { get; set; }
        [Key]
        [Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }
    }
}