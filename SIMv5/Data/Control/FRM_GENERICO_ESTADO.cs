namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.FRM_GENERICO_ESTADO")]
    public partial class FRM_GENERICO_ESTADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FRM_GENERICO_ESTADO()
        {
            VIGENCIA_SOLUCION = new HashSet<VIGENCIA_SOLUCION>();
        }

        [Key]
        public decimal ID_ESTADO { get; set; }

        public decimal? ID_ENCUESTA { get; set; }

        [StringLength(20)]
        public string COD_USURARIO { get; set; }

        public decimal TIPO_GUARDADO { get; set; }

        public decimal? ID_TERCERO { get; set; }

        public decimal? ID_INSTALACION { get; set; }

        [StringLength(200)]
        public string NOMBRE { get; set; }

        public decimal? ID_VIGENCIA { get; set; }

        public decimal? VALOR { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(1)]
        public string RADICADO { get; set; }

        public decimal? CODRADICADO { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        public int? CODTRAMITE { get; set; }

        [StringLength(50)]
        public string S_CLAVE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VIGENCIA_SOLUCION> VIGENCIA_SOLUCION { get; set; }
    }
}
