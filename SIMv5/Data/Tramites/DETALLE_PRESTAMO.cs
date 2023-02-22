namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.DETALLE_PRESTAMO")]
    public partial class DETALLE_PRESTAMO
    {
        [Key]
        public decimal ID_DETALLEPRESTAMO { get; set; }

        public decimal ID_PRESTAMO { get; set; }

        public decimal? ID_ANEXO { get; set; }

        public decimal? ID_TOMO { get; set; }

        public DateTime? D_DEVOLUCION { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        [ForeignKey("ID_PRESTAMO")]
        public virtual PRESTAMO_EXPEDIENTE PRESTAMO_EXPEDIENTE { get; set; }
    }
}
