namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_ENCUESTA")]
    public partial class EVALUACION_ENCUESTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVALUACION_ENCUESTA()
        {
            EVALUACION_RESPUESTA = new HashSet<EVALUACION_RESPUESTA>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_EVALUACION_TERCERO { get; set; }

        public int ID_INSTALACION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        [StringLength(1)]
        public string S_RESULTADO { get; set; }

        public DateTime? D_FECHA_GENERACION { get; set; }

        public int? ID_USUARIO_GENERACION { get; set; }

        [StringLength(80)]
        public string S_COORDENADA { get; set; }

        [StringLength(120)]
        public string S_DIRECCION { get; set; }

        public decimal? N_CO2P { get; set; }

        public decimal? N_CO2I { get; set; }

        [StringLength(4000)]
        public string S_OBSERVACIONES { get; set; }

        [StringLength(1)]
        public string S_EXCLUIR { get; set; }

        public decimal? N_PM25P { get; set; }

        public decimal? N_PM25I { get; set; }

        [StringLength(1)]
        public string S_PRINCIPAL { get; set; }

        [ForeignKey("ID_EVALUACION_TERCERO")]
        public virtual EVALUACION_ENCUESTA_TERCERO EVALUACION_ENCUESTA_TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVALUACION_RESPUESTA> EVALUACION_RESPUESTA { get; set; }
    }
}
