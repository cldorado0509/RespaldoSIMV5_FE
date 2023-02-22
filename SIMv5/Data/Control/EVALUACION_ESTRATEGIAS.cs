namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_ESTRATEGIAS")]
    public partial class EVALUACION_ESTRATEGIAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVALUACION_ESTRATEGIAS()
        {
            EVALUACION_ESTRATEGIAS_T = new HashSet<EVALUACION_ESTRATEGIAS_T>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int ID_GRUPO { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_ESTRATEGIAS_T> EVALUACION_ESTRATEGIAS_T { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual EVALUACION_ESTRATEGIAS_GRUPO EVALUACION_ESTRATEGIAS_GRUPO { get; set; }
    }
}
