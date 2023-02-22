namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.APLICACION")]
    public partial class APLICACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APLICACION()
        {
            MODULO = new HashSet<MODULO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_APLICACION { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(200)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(128)]
        public string S_BLOQUEADO { get; set; }

        [StringLength(2000)]
        public string S_IMAGEN { get; set; }

        public int ORDEN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MODULO> MODULO { get; set; }
    }
}
