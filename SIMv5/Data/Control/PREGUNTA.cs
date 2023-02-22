namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PREGUNTA")]
    public partial class PREGUNTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PREGUNTA()
        {
            RESPUESTA = new HashSet<RESPUESTA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PREGUNTA { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        public int ID_TIPOPREGUNTA { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(1)]
        public string S_CAMPORESPUESTA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RESPUESTA> RESPUESTA { get; set; }

        [ForeignKey("ID_TIPOPREGUNTA")]
        public virtual TIPO_PREGUNTA TIPO_PREGUNTA { get; set; }
    }
}
