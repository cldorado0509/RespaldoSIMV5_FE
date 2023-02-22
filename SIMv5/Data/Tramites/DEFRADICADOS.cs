namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.DEFRADICADOS")]
    public partial class DEFRADICADOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEFRADICADOS()
        {
            RADICADO_DOCUMENTO = new HashSet<RADICADO_DOCUMENTO>();
            RADICADO_UNIDADDOC = new HashSet<RADICADO_UNIDADDOC>();
        }

        [Key]
        public decimal ID_RADICADO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(10)]
        public string S_FORMATO1 { get; set; }

        [StringLength(10)]
        public string S_SEPARADOR { get; set; }

        [StringLength(10)]
        public string S_FORMATO2 { get; set; }

        [StringLength(512)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_REINICIOANUAL { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RADICADO_DOCUMENTO> RADICADO_DOCUMENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RADICADO_UNIDADDOC> RADICADO_UNIDADDOC { get; set; }
    }
}
