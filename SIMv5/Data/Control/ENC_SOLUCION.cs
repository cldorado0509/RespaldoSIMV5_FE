namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_SOLUCION")]
    public partial class ENC_SOLUCION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENC_SOLUCION()
        {
            ENC_SOLUCION_PREGUNTAS = new HashSet<ENC_SOLUCION_PREGUNTAS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SOLUCION { get; set; }

        public int ID_ENCUESTA { get; set; }

        public DateTime? D_SOLUCION { get; set; }

        public int? ID_ESTADO { get; set; }

        public int? ID_FORMULARIO { get; set; }

        public decimal? N_VALOR { get; set; }

        public DateTime? D_EDICION { get; set; }

        [ForeignKey("ID_ENCUESTA")]
        public virtual ENC_ENCUESTA ENC_ENCUESTA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENC_SOLUCION_PREGUNTAS> ENC_SOLUCION_PREGUNTAS { get; set; }

        [ForeignKey("ID_FORMULARIO")]
        public virtual FORMULARIO FORMULARIO { get; set; }
    }
}
