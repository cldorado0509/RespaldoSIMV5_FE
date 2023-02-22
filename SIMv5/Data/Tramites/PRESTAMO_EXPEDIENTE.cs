namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PRESTAMO_EXPEDIENTE")]
    public partial class PRESTAMO_EXPEDIENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRESTAMO_EXPEDIENTE()
        {
            DETALLE_PRESTAMO = new HashSet<DETALLE_PRESTAMO>();
        }

        [Key]
        public decimal ID_PRESTAMO { get; set; }

        public DateTime D_PRESTAMO { get; set; }

        public decimal CODFUNCIONARIO { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public int? ID_PRESTAMOCONTROLEXPEDIENTE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETALLE_PRESTAMO> DETALLE_PRESTAMO { get; set; }
    }
}
