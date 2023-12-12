namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS")]
    public partial class PMES_ESTRATEGIAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PMES_ESTRATEGIAS()
        {
            PMES_ESTRATEGIAS_TP = new HashSet<PMES_ESTRATEGIAS_TP>();
            PMES_ESTRATEGIAS_TF = new HashSet<PMES_ESTRATEGIAS_TF>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int ID_GRUPO { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMES_ESTRATEGIAS_TP> PMES_ESTRATEGIAS_TP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMES_ESTRATEGIAS_TF> PMES_ESTRATEGIAS_TF { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual PMES_ESTRATEGIAS_GRUPO PMES_ESTRATEGIAS_GRUPO { get; set; }
    }
}
