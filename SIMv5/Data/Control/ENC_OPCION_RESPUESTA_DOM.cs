namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_OPCION_RESPUESTA_DOM")]
    public partial class ENC_OPCION_RESPUESTA_DOM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENC_OPCION_RESPUESTA_DOM()
        {
            ENC_PREGUNTA = new HashSet<ENC_PREGUNTA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_OPCION_RESPUESTA_DOM { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(2048)]
        public string S_SQL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENC_PREGUNTA> ENC_PREGUNTA { get; set; }
    }
}
