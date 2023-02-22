namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PRESTAMO_DETALLE")]
    public partial class PRESTAMO_DETALLE_TRAMITES
    {
        [Key]
        public decimal ID_PRESDETALLE { get; set; }

        public decimal ID_PRESTAMO { get; set; }

        public decimal ID_TIPOPRESTAMO { get; set; }

        public decimal ID_IDENTIFICADOR { get; set; }

        public DateTime? D_FECHADEVOLUCION { get; set; }

        public decimal? ID_FUNCRECIBE { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1000)]
        public string S_OBSERDEVOLUCION { get; set; }

        public DateTime? D_FECHAHASTA { get; set; }

        [StringLength(50)]
        public string S_REFERENCIA { get; set; }

        [ForeignKey("ID_PRESTAMO")]
        public virtual PRESTAMOS_TRAMITES PRESTAMOS { get; set; }

        [ForeignKey("ID_TIPOPRESTAMO")]
        public virtual PRESTAMO_TIPO PRESTAMO_TIPO { get; set; }

        [ForeignKey("ID_FUNCRECIBE")]
        public virtual TBFUNCIONARIO TBFUNCIONARIO { get; set; }
    }
}
