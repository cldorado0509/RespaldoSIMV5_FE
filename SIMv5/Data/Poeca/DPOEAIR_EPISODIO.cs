namespace SIM.Data.Poeca
{
    using DevExpress.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.DPOEAIR_EPISODIO")]
    public partial class DPOEAIR_EPISODIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DPOEAIR_EPISODIO()
        {
            TPOEAIR_SEGUIMIENTO_META = new HashSet<TPOEAIR_SEGUIMIENTO_META>();
            TPOEAIR_SEGUIMIENTO_GLOBAL = new HashSet<TPOEAIR_SEGUIMIENTO_GLOBAL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Año del Episodio")]
        public int N_ANIO { get; set; }

        [Required]
        [Display(Name = "Período de implementación")]
        public int ID_PERIODO { get; set; }

        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string S_OBSERVACIONES { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual PeriodoImplementacion DPOEAIR_PERIODO_IMPLEMENTACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_SEGUIMIENTO_GLOBAL> TPOEAIR_SEGUIMIENTO_GLOBAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_SEGUIMIENTO_META> TPOEAIR_SEGUIMIENTO_META { get; set; }
    }
}
