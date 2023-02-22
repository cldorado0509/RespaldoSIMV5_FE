namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PRESTAMO_TIPO")]
    public partial class PRESTAMO_TIPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRESTAMO_TIPO()
        {
            PRESTAMO_DETALLE = new HashSet<PRESTAMO_DETALLE_TRAMITES>();
        }

        [Key]
        public decimal ID_TIPOPRESTAMO { get; set; }

        [Required]
        [StringLength(255)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(50)]
        public string S_TABLAREF { get; set; }

        [StringLength(50)]
        public string S_CAMPOIDREF { get; set; }

        [StringLength(50)]
        public string S_CAMPOBUSCAR { get; set; }

        [StringLength(255)]
        public string S_SQL { get; set; }

        [StringLength(255)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRESTAMO_DETALLE_TRAMITES> PRESTAMO_DETALLE { get; set; }
    }
}
