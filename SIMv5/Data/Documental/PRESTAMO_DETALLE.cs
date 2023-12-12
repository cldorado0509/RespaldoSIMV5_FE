namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.PRESTAMO_DETALLE")]
    public partial class PRESTAMO_DETALLE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PRESTAMODETALLE { get; set; }

        public int ID_PRESTAMO { get; set; }

        public int? ID_TIPOPRESTAMO { get; set; }

        public int? ID_IDENTIFICADOR { get; set; }

        public DateTime? D_DEVOLUCION { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1000)]
        public string S_OBSERVACIONDEVOLUCION { get; set; }

        public DateTime? D_HASTA { get; set; }

        [StringLength(200)]
        public string S_REFERENCIA { get; set; }

        public int? ID_TERCERORECIBE { get; set; }

        public int? ID_EXPEDIENTE { get; set; }

        public int? ID_TOMO { get; set; }

        public int? ID_DOCUMENTO { get; set; }

        public int? ID_ANEXO { get; set; }

        public int ID_RADICADO { get; set; }

        public DateTime? D_NOTIFICACION { get; set; }
    }
}
