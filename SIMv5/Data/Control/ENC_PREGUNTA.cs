namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_PREGUNTA")]
    public partial class ENC_PREGUNTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENC_PREGUNTA()
        {
            ENC_OPCION_RESPUESTA = new HashSet<ENC_OPCION_RESPUESTA>();
            ENC_SOLUCION_PREGUNTAS = new HashSet<ENC_SOLUCION_PREGUNTAS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PREGUNTA { get; set; }

        [StringLength(1000)]
        public string S_NOMBRE { get; set; }

        public int ID_TIPOPREGUNTA { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(2000)]
        public string S_AYUDA { get; set; }

        [StringLength(250)]
        public string SQL_OPCION_EXTERNO { get; set; }

        public int? ID_OPCION_RESPUESTA_DOM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENC_OPCION_RESPUESTA> ENC_OPCION_RESPUESTA { get; set; }

        [ForeignKey("ID_OPCION_RESPUESTA_DOM")]
        public virtual ENC_OPCION_RESPUESTA_DOM ENC_OPCION_RESPUESTA_DOM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENC_SOLUCION_PREGUNTAS> ENC_SOLUCION_PREGUNTAS { get; set; }

        [ForeignKey("ID_TIPOPREGUNTA")]
        public virtual ENC_TIPO_PREGUNTA ENC_TIPO_PREGUNTA { get; set; }
    }
}
