namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBSERIE_DOCUMENTAL")]
    public partial class TBSERIE_DOCUMENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBSERIE_DOCUMENTAL()
        {
            TBSUBSERIE_DOCUMENTAL = new HashSet<TBSUBSERIE_DOCUMENTAL>();
        }

        [Key]
        public decimal CODSERIE_DOCUMENTAL { get; set; }

        [Required]
        [StringLength(1000)]
        public string NOMBRE { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        [StringLength(1)]
        public string ELIMINADO { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(1)]
        public string RADICADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBSUBSERIE_DOCUMENTAL> TBSUBSERIE_DOCUMENTAL { get; set; }
    }
}
