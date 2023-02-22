namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INFESTADO")]
    public partial class INFESTADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INFESTADO()
        {
            INFORME_TECNICO = new HashSet<INFORME_TECNICO>();
        }

        [Key]
        public decimal ID_ESTADO { get; set; }

        [StringLength(20)]
        public string NOMBRE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INFORME_TECNICO> INFORME_TECNICO { get; set; }
    }
}
