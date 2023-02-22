namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_PREGUNTA_GRUPO")]
    public partial class EVALUACION_PREGUNTA_GRUPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVALUACION_PREGUNTA_GRUPO()
        {
            EVALUACION_PREGUNTA = new HashSet<EVALUACION_PREGUNTA>();
            EVALUACION_PREGUNTA_GRUPO1 = new HashSet<EVALUACION_PREGUNTA_GRUPO>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_EVALUACION_TIPO { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        public int N_ORDEN { get; set; }

        public int? ID_GRUPO_PADRE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_PREGUNTA> EVALUACION_PREGUNTA { get; set; }

        [ForeignKey("ID_EVALUACION_TIPO")]
        public virtual EVALUACION_TIPO EVALUACION_TIPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_PREGUNTA_GRUPO> EVALUACION_PREGUNTA_GRUPO1 { get; set; }

        [ForeignKey("ID_GRUPO_PADRE")]
        public virtual EVALUACION_PREGUNTA_GRUPO EVALUACION_PREGUNTA_GRUPO2 { get; set; }
    }
}
