namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_ENCUESTA_TERCERO")]
    public partial class EVALUACION_ENCUESTA_TERCERO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVALUACION_ENCUESTA_TERCERO()
        {
            EVALUACION_ENCUESTA = new HashSet<EVALUACION_ENCUESTA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_EVALUACION_TIPO { get; set; }

        public int ID_TERCERO { get; set; }

        [Required]
        [StringLength(20)]
        public string S_VALOR_VIGENCIA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        public DateTime? D_FECHA_GENERACION { get; set; }

        public int? ID_USUARIO_GENERACION { get; set; }

        [StringLength(1)]
        public string S_RESULTADO { get; set; }

        public int ID_ESTADO { get; set; }

        public int CODTRAMITE { get; set; }

        [StringLength(1)]
        public string S_MEDIO_ENTREGA { get; set; }

        [StringLength(20)]
        public string S_RADICADO { get; set; }

        public DateTime? D_FECHA_ENTREGA { get; set; }

        [StringLength(20)]
        public string S_CM { get; set; }

        public int? N_COD_MUNICIPIO { get; set; }

        [StringLength(1)]
        public string S_VERSION { get; set; }

        [StringLength(4000)]
        public string S_OBSERVACIONES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_ENCUESTA> EVALUACION_ENCUESTA { get; set; }
    }
}
