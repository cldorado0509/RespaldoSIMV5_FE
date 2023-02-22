namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_TIPO")]
    public partial class EVALUACION_TIPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVALUACION_TIPO()
        {
            EVALUACION_PREGUNTA_GRUPO = new HashSet<EVALUACION_PREGUNTA_GRUPO>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(120)]
        public string S_NOMBRE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_PREGUNTA_GRUPO> EVALUACION_PREGUNTA_GRUPO { get; set; }
    }
}
