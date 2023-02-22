namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.QRY_INDICESDESPACHADOS")]
    public partial class QRY_INDICESDESPACHADOS
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }

        [StringLength(510)]
        public string RADICADO { get; set; }

        [Column("FECHA RADICADO")]
        public DateTime? FECHA_RADICADO { get; set; }

        [StringLength(510)]
        public string ASUNTO { get; set; }

        [StringLength(510)]
        public string DESTINATARIO { get; set; }

        [StringLength(510)]
        public string REMITENTE { get; set; }

        [StringLength(510)]
        public string EMPRESA { get; set; }

        [StringLength(510)]
        public string DIRECCION { get; set; }

        [StringLength(510)]
        public string MUNICIPIO { get; set; }

        [StringLength(510)]
        public string CARGO { get; set; }

        [StringLength(510)]
        public string CENTROCOSTO { get; set; }
    }
}
