namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBPROCESO")]
    public partial class TBPROCESO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBPROCESO()
        {
            TBTAREA = new HashSet<TBTAREA>();
            TBTAREA1 = new HashSet<TBTAREA>();
            TBTRAMITE = new HashSet<TBTRAMITE>();
        }

        [Key]
        public decimal CODPROCESO { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public decimal? TIEMPO { get; set; }

        public int? VALIDO { get; set; }

        [StringLength(4000)]
        public string INFORMACION { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(1)]
        public string TALAPODA { get; set; }

        public int RADICADO_ACTUAL { get; set; }

        [Required]
        [StringLength(1)]
        public string INICIA_AUTOMATICO { get; set; }

        [StringLength(1)]
        public string S_MARCATAREA { get; set; }

        [StringLength(1)]
        public string S_PRIORITARIO { get; set; }

        [StringLength(20)]
        public string S_COLORMARCA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTAREA> TBTAREA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTAREA> TBTAREA1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTRAMITE> TBTRAMITE { get; set; }
    }
}
