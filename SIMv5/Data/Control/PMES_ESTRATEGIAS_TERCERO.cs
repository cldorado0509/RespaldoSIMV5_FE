namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_TERCERO")]
    public partial class PMES_ESTRATEGIAS_TERCERO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PMES_ESTRATEGIAS_TERCERO()
        {
            PMES_ESTRATEGIAS_TF = new HashSet<PMES_ESTRATEGIAS_TF>();
            PMES_ESTRATEGIAS_TP = new HashSet<PMES_ESTRATEGIAS_TP>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_ESTADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMES_ESTRATEGIAS_TF> PMES_ESTRATEGIAS_TF { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMES_ESTRATEGIAS_TP> PMES_ESTRATEGIAS_TP { get; set; }
    }
}
