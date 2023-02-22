namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_ENCUESTA")]
    public partial class ENC_ENCUESTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENC_ENCUESTA()
        {
            ENC_SOLUCION = new HashSet<ENC_SOLUCION>();
            ENCUESTA_VIGENCIA = new HashSet<ENCUESTA_VIGENCIA>();
            FORMULARIO_ENCUESTA = new HashSet<FORMULARIO_ENCUESTA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ENCUESTA { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        public DateTime? D_CREACION { get; set; }

        public int? ID_USUARIO { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENC_SOLUCION> ENC_SOLUCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENCUESTA_VIGENCIA> ENCUESTA_VIGENCIA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORMULARIO_ENCUESTA> FORMULARIO_ENCUESTA { get; set; }
    }
}
