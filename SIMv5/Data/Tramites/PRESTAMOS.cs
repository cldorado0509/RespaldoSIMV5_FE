namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PRESTAMOS")]
    public partial class PRESTAMOS_TRAMITES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRESTAMOS_TRAMITES()
        {
            PRESTAMO_DETALLE = new HashSet<PRESTAMO_DETALLE_TRAMITES>();
        }

        [Key]
        public decimal ID_PRESTAMO { get; set; }

        public decimal ID_TERCEROPRESTAMO { get; set; }

        public DateTime D_FECHAPRESTAMO { get; set; }

        public decimal ID_FUNCPRESTA { get; set; }

        [StringLength(1000)]
        public string S_OBSERVACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRESTAMO_DETALLE_TRAMITES> PRESTAMO_DETALLE { get; set; }

        [ForeignKey("ID_FUNCPRESTA")]
        public virtual TBFUNCIONARIO TBFUNCIONARIO { get; set; }
    }
}
