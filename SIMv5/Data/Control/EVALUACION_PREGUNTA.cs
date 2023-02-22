namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_PREGUNTA")]
    public partial class EVALUACION_PREGUNTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVALUACION_PREGUNTA()
        {
            EVALUACION_RESPUESTA = new HashSet<EVALUACION_RESPUESTA>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ID_GRUPO { get; set; }

        [Required]
        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        public int N_TIPO_RESPUESTA { get; set; }

        public int? N_ORDEN { get; set; }

        [StringLength(1)]
        public string S_CALCULADO { get; set; }

        public int? N_VALOR_COMPLEMENTO { get; set; }

        public int? ID_GRUPO_COMPLEMENTO { get; set; }

        [StringLength(1)]
        public string S_VERSION { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual EVALUACION_PREGUNTA_GRUPO EVALUACION_PREGUNTA_GRUPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_RESPUESTA> EVALUACION_RESPUESTA { get; set; }
    }
}
